using PasswordManager.PaymentCards.ApplicationServices.Operations;
using PasswordManager.PaymentCards.Domain.Operations;
using PaymentCards.Messages.UpdatePaymentCard;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.UpdatePaymentCard;
using PaymentCards.Worker.Service.UpdatePaymentCard;
using Rebus.Handlers;

namespace PaymentCards.Worker.Service.UpdatePaymentCard
{
    public class UpdatePaymentCardCommandHandler : IHandleMessages<UpdatePaymentCardCommand>
    {
        private readonly IUpdatePaymentCardService _updatePaymentCardService;
        private readonly IOperationService _operationService;
        private readonly ILogger<UpdatePaymentCardCommandHandler> _logger;

        public UpdatePaymentCardCommandHandler(IUpdatePaymentCardService updatePaymentCardService, IOperationService operationService, ILogger<UpdatePaymentCardCommandHandler> logger)
        {
            _updatePaymentCardService = updatePaymentCardService;
            _operationService = operationService;
            _logger = logger;
        }

        public async Task Handle(UpdatePaymentCardCommand message)
        {
            _logger.LogInformation($"Handling update of PaymentCard command: {message.RequestId}");

            var operation = await _operationService.GetOperationByRequestId(message.RequestId);

            if (operation == null)
            {
                _logger.LogError($"Operation not found: {message.RequestId}");
                return;
            }

            await _operationService.UpdateOperationStatus(message.RequestId, OperationStatus.Processing);

            var paymentCardModel = UpdatePaymentCardOperationHelper.Map(operation.PaymentCardId, operation);

            if (paymentCardModel == null)
            {
                _logger.LogError($"PaymentCard model not found: {operation.PaymentCardId}");
                return;
            }

            await _updatePaymentCardService.UpdatePaymentCard(paymentCardModel);

            await _operationService.UpdateOperationStatus(message.RequestId, OperationStatus.Completed);

            _logger.LogInformation($"PaymentCard updated: {operation.PaymentCardId}");

            OperationResult.Completed(operation);
        }
    }
}
