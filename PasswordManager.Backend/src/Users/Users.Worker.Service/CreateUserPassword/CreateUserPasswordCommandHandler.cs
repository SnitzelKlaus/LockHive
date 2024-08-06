using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
using PasswordManager.Users.Domain.Operations;
using Rebus.Bus;
using Rebus.Handlers;
using Users.Messages.CreateUserPassword;

namespace Users.Worker.Service.CreateUserPassword;

/// <summary>
/// Handles commands to create user passwords, interfacing with relevant services and publishing events based on the outcome.
/// </summary>
public class CreateUserPasswordCommandHandler : IHandleMessages<CreateUserPasswordCommand>
{
    private readonly ICreateUserPasswordService _createUserPasswordService;
    private readonly IOperationService _operationService;
    private readonly ILogger<CreateUserPasswordCommandHandler> _logger;
    private readonly IBus _bus;

    /// <summary>
    /// Initializes a new instance of the CreateUserPasswordCommandHandler class.
    /// </summary>
    /// <param name="createUserPasswordService">The service to create user passwords.</param>
    /// <param name="operationService">The service managing operations.</param>
    /// <param name="logger">The logger for logging information and warnings.</param>
    /// <param name="bus">The bus for publishing events.</param>
    public CreateUserPasswordCommandHandler(ICreateUserPasswordService createUserPasswordService, IOperationService operationService, ILogger<CreateUserPasswordCommandHandler> logger, IBus bus)
    {
        _createUserPasswordService = createUserPasswordService;
        _operationService = operationService;
        _logger = logger;
        _bus = bus;
    }

    /// <summary>
    /// Handles the CreateUserPasswordCommand message asynchronously.
    /// </summary>
    /// <param name="message">The command message containing the request ID for creating a user password.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(CreateUserPasswordCommand message)
    {
        var requestId = message.RequestId;
        _logger.LogInformation($"Handling create user password command: {requestId}");

        var operation = await _operationService.GetOperationByRequestId(requestId);

        if (operation is null)
        {
            _logger.LogWarning($"Operation not found: {requestId}");
            throw new InvalidOperationException($"Could not find operation with request id {requestId} when creating user password");
        }

        await _operationService.UpdateOperationStatus(requestId, OperationStatus.Processing);

        var createUserPasswordModel = CreateUserPasswordOperationHelper.Map(operation.UserId, operation);

        try
        {
            await _createUserPasswordService.CreateUserPassword(createUserPasswordModel);
            await PublishSuccessEventAndMarkOperationAsCompleted(createUserPasswordModel.UserId, requestId);

            OperationResult.Completed(operation);
        }
        catch (CreateUserPasswordServiceException exception)
        {
            await PublishFailedEventAndMarkOperationAsFailed(createUserPasswordModel.UserId, requestId, exception.Message);
        }
    }

    private async Task PublishFailedEventAndMarkOperationAsFailed(Guid userId, string requestId, string message)
    {
        await _bus.Publish(new CreateUserPasswordFailedEvent(userId, requestId, message ?? string.Empty));
        await SetOperationStatus(requestId, OperationStatus.Failed);
    }

    private async Task PublishSuccessEventAndMarkOperationAsCompleted(Guid userId, string requestId)
    {
        await _operationService.UpdateOperationStatus(requestId, OperationStatus.Completed);
        await _bus.Publish(new CreateUserPasswordEvent(userId, requestId));
    }

    private async Task SetOperationStatus(string requestId, OperationStatus operationStatus)
    {
        await _operationService.UpdateOperationStatus(requestId, operationStatus);
    }
}
