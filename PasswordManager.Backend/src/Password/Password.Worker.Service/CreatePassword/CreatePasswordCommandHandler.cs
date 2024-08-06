using Rebus.Handlers;
using Password.Messages.CreatePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.Domain.Operations;
using Rebus.Bus;
using PasswordManager.Password.ApplicationServices.Password.CreatePassword;

namespace Password.Worker.Service.CreatePassword;

public sealed class CreatePasswordCommandHandler : IHandleMessages<CreatePasswordCommand>
{
    private readonly ICreatePasswordService _createPasswordService;
    private readonly IOperationService _operationService;
    private readonly ILogger<CreatePasswordCommandHandler> _logger;
    private readonly IBus _bus;

    public CreatePasswordCommandHandler(ICreatePasswordService createPasswordService, IOperationService operationService, ILogger<CreatePasswordCommandHandler> logger, IBus bus)
    {
        _createPasswordService = createPasswordService;
        _operationService = operationService;
        _logger = logger;
        _bus = bus;
    }

    public async Task Handle(CreatePasswordCommand message)
    {
        var requestId = message.RequestId;
        _logger.LogInformation($"Handling create password command: {requestId}");

        var operation = await _operationService.GetOperationByRequestId(requestId);

        if (operation == null)
        {
            _logger.LogWarning($"Operation not found: {requestId}");
            throw new InvalidOperationException($"Could not find operation with request id {requestId} when creating password");
        }

        await _operationService.UpdateOperationStatus(requestId, OperationStatus.Processing);

        var createPasswordModel = CreatePasswordOperationHelper.Map(operation.PasswordId, operation);

        try
        {
            await _createPasswordService.CreatePassword(createPasswordModel);
            await PublishSuccessEventAndMarkOperationAsCompleted(createPasswordModel.Id, requestId);
            OperationResult.Completed(operation);
        }
        catch (CreatePasswordServiceException exception)
        {
            await PublishFailedEventAndMarkOperationAsFailed(createPasswordModel.Id, requestId, exception.Message);
        }
    }

    private async Task PublishFailedEventAndMarkOperationAsFailed(Guid passwordId, string requestId, string message)
    {
        await _bus.Publish(new CreatePasswordFailedEvent(passwordId, requestId, message ?? string.Empty));
        await SetOperationStatus(requestId, OperationStatus.Failed);
    }

    private async Task PublishSuccessEventAndMarkOperationAsCompleted(Guid passwordId, string requestId)
    {
        await _operationService.UpdateOperationStatus(requestId, OperationStatus.Completed);
        await _bus.Publish(new CreatePasswordEvent(passwordId, requestId));
    }

    private async Task SetOperationStatus(string requestId, OperationStatus operationStatus)
    {
        await _operationService.UpdateOperationStatus(requestId, operationStatus);
    }
}
