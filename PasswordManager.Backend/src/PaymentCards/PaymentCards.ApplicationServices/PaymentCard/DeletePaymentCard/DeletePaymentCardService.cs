using Microsoft.Extensions.Logging;
using PasswordManager.PaymentCards.ApplicationServices.Operations;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.CreatePaymentCard;
using PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;
using PaymentCards.Messages.DeletePaymetCard;
using Rebus.Bus;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.DeletePaymentCard
{
    public class DeletePaymentCardService : IDeletePaymentCardService
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IOperationService _operationService;
        private readonly ILogger<DeletePaymentCardService> _logger;
        private readonly IBus _bus;

        public DeletePaymentCardService(IPaymentCardRepository paymentCardRepository, IOperationService operationService,
            ILogger<DeletePaymentCardService> logger, IBus bus)
        {
            _paymentCardRepository = paymentCardRepository;
            _operationService = operationService;
            _logger = logger;
            _bus = bus;
        }

        public async Task<OperationResult> RequestDeletePaymentCard(Guid paymentCardId, OperationDetails operationDetails)
        {
            _logger.LogInformation($"Request deletion of PaymentCard with ID: {paymentCardId}");

            // Creates the operation
            var operation = await _operationService.QueueOperation(OperationBuilder.DeletePaymentCard(paymentCardId, operationDetails.CreatedBy));

            // Sends the operation to the bus
            await _bus.Send(new DeletePaymentCardCommand(operation.RequestId));

            _logger.LogInformation($"Request added to servicebus with request ID: {operation.RequestId}, PaymentCard ID: {paymentCardId}");

            return OperationResult.Accepted(operation);
        }

        public async Task DeletePaymentCard(Guid paymentCardId)
        {
            _logger.LogInformation($"Deleting PaymentCard with ID: {paymentCardId}");

            await _paymentCardRepository.Delete(paymentCardId);

            _logger.LogInformation($"PaymentCard with ID: {paymentCardId} deleted");
            return;
        }
    }
}
