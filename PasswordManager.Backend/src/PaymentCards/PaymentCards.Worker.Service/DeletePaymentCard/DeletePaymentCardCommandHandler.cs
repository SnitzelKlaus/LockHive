using Rebus.Handlers;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.DeletePaymentCard;
using PaymentCards.Messages.DeletePaymetCard;
using PasswordManager.PaymentCards.ApplicationServices.Operations;
using PasswordManager.PaymentCards.Domain.Operations;

namespace PaymentCards.Worker.Service.DeletePaymentCard
{
    public class DeletePaymentCardCommandHandler : IHandleMessages<DeletePaymentCardCommand>
    {
        private readonly IDeletePaymentCardService _deletePaymentCardService;
        private readonly IOperationService _operationService;
        private readonly ILogger<DeletePaymentCardCommandHandler> _logger;

        public DeletePaymentCardCommandHandler(IDeletePaymentCardService deletePaymentCardService, IOperationService operationService, ILogger<DeletePaymentCardCommandHandler> logger)
        {
            _deletePaymentCardService = deletePaymentCardService;
            _operationService = operationService;
            _logger = logger;
        }

        public async Task Handle(DeletePaymentCardCommand message)
        {
            _logger.LogInformation($"Handling deletion of PaymentCard command: {message.RequestId}");

            var operation = await _operationService.GetOperationByRequestId(message.RequestId);

            if (operation == null)
            {
                _logger.LogError($"Operation not found: {message.RequestId}");
                return;
            }

            await _operationService.UpdateOperationStatus(message.RequestId, OperationStatus.Processing);

            await _deletePaymentCardService.DeletePaymentCard(operation.PaymentCardId);

            await _operationService.UpdateOperationStatus(message.RequestId, OperationStatus.Completed);

            _logger.LogInformation($"PaymentCard deleted: {operation.PaymentCardId}");

            OperationResult.Completed(operation);
        }
    }
}
