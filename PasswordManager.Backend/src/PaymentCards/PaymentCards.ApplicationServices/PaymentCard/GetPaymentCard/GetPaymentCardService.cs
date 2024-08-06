using Microsoft.Extensions.Logging;
using PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.GetPaymentCard
{
    public class GetPaymentCardService : IGetPaymentCardService
    {
        private readonly IPaymentCardRepository _paymentCardRepository;

        public GetPaymentCardService(IPaymentCardRepository paymentCardRepository)
        {
            _paymentCardRepository = paymentCardRepository;
        }

        public async Task<PaymentCardModel?> GetPaymentCardById(Guid paymentCardId) => await _paymentCardRepository.GetById(paymentCardId);

        public Task<IEnumerable<PaymentCardModel>?> GetPaymentCardsByUserId(Guid userId) => _paymentCardRepository.GetByUserId(userId);
    }
}
