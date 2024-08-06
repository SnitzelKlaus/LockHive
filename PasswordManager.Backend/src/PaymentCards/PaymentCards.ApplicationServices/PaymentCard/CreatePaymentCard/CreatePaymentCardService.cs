using Microsoft.Extensions.Logging;
using PasswordManager.PaymentCards.ApplicationServices.Operations;
using PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;
using PaymentCards.Messages.CreatePaymentCard;
using Rebus.Bus;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.CreatePaymentCard
{
    public sealed class CreatePaymentCardService : ICreatePaymentCardService
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IOperationService _operationService;
        private readonly ILogger<CreatePaymentCardService> _logger;
        private readonly IBus _bus;

        public CreatePaymentCardService(IPaymentCardRepository paymentCardRepository, IOperationService operationService, 
            ILogger<CreatePaymentCardService> logger, IBus bus)
        {
            _paymentCardRepository = paymentCardRepository;
            _operationService = operationService;
            _logger = logger;
            _bus = bus;
        }

        public async Task<OperationResult> RequestCreatePaymentCard(PaymentCardModel paymentCardModel, OperationDetails operationDetails)
        {
            _logger.LogInformation($"Request to create PaymentCard for user: {paymentCardModel.UserId}");

            // Creates the operation
            var operation = await _operationService.QueueOperation(OperationBuilder.CreatePaymentCard(paymentCardModel, operationDetails.CreatedBy));

            // Sends the operation to the bus
            await _bus.Send(new CreatePaymentCardCommand(operation.RequestId));

            _logger.LogInformation($"Request added to servicebus with request ID: {operation.RequestId}, PaymentCard ID: {paymentCardModel.Id}");

            return OperationResult.Accepted(operation);
        }

        public async Task CreatePaymentCard(PaymentCardModel paymentCardModel)
        {
            _logger.LogInformation($"Creating PaymentCard with ID: {paymentCardModel.Id}");

            await _paymentCardRepository.Upsert(paymentCardModel);

            _logger.LogInformation($"PaymentCard with ID: {paymentCardModel.Id} created");
            return;
        }
    }
}
