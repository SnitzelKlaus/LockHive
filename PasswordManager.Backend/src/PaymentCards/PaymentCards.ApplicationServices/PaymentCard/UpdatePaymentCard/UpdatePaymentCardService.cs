using Microsoft.Extensions.Logging;
using PasswordManager.PaymentCards.ApplicationServices.Operations;
using PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;
using PaymentCards.Messages.UpdatePaymentCard;
using Rebus.Bus;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.UpdatePaymentCard
{
    public class UpdatePaymentCardService : IUpdatePaymentCardService
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IOperationService _operationService;
        private readonly ILogger<UpdatePaymentCardService> _logger;
        private readonly IBus _bus;

        public UpdatePaymentCardService(IPaymentCardRepository paymentCardRepository, IOperationService operationService,
            ILogger<UpdatePaymentCardService> logger, IBus bus)
        {
            _paymentCardRepository = paymentCardRepository;
            _operationService = operationService;
            _logger = logger;
            _bus = bus;
        }

        public async Task<OperationResult> RequestUpdatePaymentCard(PaymentCardModel paymentCardModel, OperationDetails operationDetails)
        {
            _logger.LogInformation("Request update PaymentCard with ID: {paymentCardModelId}", paymentCardModel.Id);

            // Checks if the payment card already exists
            var existingPaymentCard = await _paymentCardRepository.GetById(paymentCardModel.Id);
            if (existingPaymentCard == null)
            {
                _logger.LogWarning("PaymentCard with ID: {paymentCardModelId}, does not exists", paymentCardModel.Id);
                return OperationResult.InvalidState("PaymentCard does not exists");
            }

            // Creates the operation
            var operation = await _operationService.QueueOperation(OperationBuilder.UpdatePaymentCard(paymentCardModel, operationDetails.CreatedBy));

            // Sends the operation to the bus
            await _bus.Send(new UpdatePaymentCardCommand(operation.RequestId));

            _logger.LogInformation("Request added to servicebus with request ID: {operationRequestId}, PaymentCard ID: {paymentCardModelId}", operation.RequestId, paymentCardModel.Id);

            return OperationResult.Accepted(operation);
        }

        public async Task UpdatePaymentCard(PaymentCardModel paymentCardModel)
        {
            _logger.LogInformation("Updating PaymentCard with ID: {paymentCardModelId}", paymentCardModel.Id);

            await _paymentCardRepository.Upsert(paymentCardModel);

            _logger.LogInformation("PaymentCard with ID: {paymentCardModelId} updated", paymentCardModel.Id);
            return;
        }
    }
}
