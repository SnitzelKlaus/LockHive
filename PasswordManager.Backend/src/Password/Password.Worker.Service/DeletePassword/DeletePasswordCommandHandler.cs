using Password.Messages.DeletePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Password.DeletePassword;
using PasswordManager.Password.Domain.Operations;
using Rebus.Handlers;

namespace Password.Worker.Service.DeletePassword
{
    public sealed class DeletePasswordCommandHandler : IHandleMessages<DeletePasswordCommand>
    {
        private readonly IDeletePasswordService _deletePasswordService;
        private readonly IOperationService _operationService;
        private readonly ILogger<DeletePasswordCommandHandler> _logger;

        public DeletePasswordCommandHandler(IDeletePasswordService deletePasswordService, IOperationService operationService, ILogger<DeletePasswordCommandHandler> logger)
        {
            _deletePasswordService = deletePasswordService;
            _operationService = operationService;
            _logger = logger;
        }

        public async Task Handle(DeletePasswordCommand message)
        {
            _logger.LogInformation($"Handling delete password command: {message.RequestId}");

            var operation = await _operationService.GetOperationByRequestId(message.RequestId);

            if (operation == null)
            {
                _logger.LogWarning($"Operation not found: {message.RequestId}");
                return;
            }

            await _operationService.UpdateOperationStatus(message.RequestId, OperationStatus.Processing);
            
            await _deletePasswordService.DeletePassword(operation.PasswordId);
            
            await _operationService.UpdateOperationStatus(message.RequestId, OperationStatus.Completed);

            OperationResult.Completed(operation);
        }
    }
}
