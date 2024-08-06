using Password.Messages.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Password.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Operations;
using Rebus.Bus;
using Rebus.Handlers;

namespace Password.Worker.Service.UpdatePassword;

public class UpdatePasswordCommandHandler : IHandleMessages<UpdatePasswordCommand>
{
    private readonly IUpdatePasswordService _updatePasswordService;
    private readonly IOperationService _operationService;
    private readonly ILogger<UpdatePasswordCommandHandler> _logger;
    private readonly IBus _bus;

    public UpdatePasswordCommandHandler(IUpdatePasswordService updateUserService, IOperationService operationService, ILogger<UpdatePasswordCommandHandler> logger, IBus bus)
    {
        _updatePasswordService = updateUserService;
        _operationService = operationService;
        _logger = logger;
        _bus = bus;
    }

    /// <summary>
    /// Handles the UpdatePasswordCommand
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task Handle(UpdatePasswordCommand message)
    {
        var requestId = message.RequestId;

        //Get operation by requestId
        var operation = await _operationService.GetOperationByRequestId(requestId);
        if (operation is null)
        {
            _logger.LogError("Could not find operation with request id {RequestId} when updating password", requestId);
            throw new InvalidOperationException($"Could not find operation with request id {requestId} when updating password");
        }

        //Update operation status to processing.
        await _operationService.UpdateOperationStatus(requestId, OperationStatus.Processing);

        //Get the updated password from the operation that has been stored
        var updatePasswordModel = UpdatePasswordOperationHelper.Map(operation.PasswordId, operation);

        try
        {
            await _updatePasswordService.UpdatePassword(updatePasswordModel);
        }
        catch (PasswordRepositoryException e)
        {
            await FailOperation(updatePasswordModel.Id, requestId, e.Message);
            throw;
        }

        await CompleteOperation(updatePasswordModel.Id, requestId, operation);
    }

    /// <summary>
    /// Completes the operation and publish a updatePasswordEvent
    /// </summary>
    /// <param name="passwordId"></param>
    /// <param name="requestId"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private async Task CompleteOperation(Guid passwordId, string requestId, Operation operation)
    {
        await SetOperationStatus(requestId, OperationStatus.Completed);
        await _bus.Publish(new UpdatePasswordEvent(passwordId, requestId));
        _logger.LogInformation("Handled request {RequestId} to update password", requestId);

        OperationResult.Completed(operation);
    }

    /// <summary>
    /// Fails the operation and publish a updatedPasswordFailedEvent
    /// </summary>
    /// <param name="passwordId"></param>
    /// <param name="requestId"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    private async Task FailOperation(Guid passwordId, string requestId, string errorMessage)
    {
        _logger.LogError("Could not complete UpdatePassword by {PasswordId}, request id {RequestId} - {ErrorMessage}", passwordId, requestId, errorMessage);
        await SetOperationStatus(requestId, OperationStatus.Failed);
        await _bus.Publish(new UpdatePasswordFailedEvent(passwordId, requestId, errorMessage));
    }

    /// <summary>
    /// Sets the operation Status
    /// </summary>
    /// <param name="requestId"></param>
    /// <param name="operationStatus"></param>
    /// <returns></returns>
    private async Task SetOperationStatus(string requestId, OperationStatus operationStatus)
    {
        await _operationService.UpdateOperationStatus(requestId, operationStatus);
    }
}
