using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.GetPaymentCard
{
    public interface IGetPaymentCardService
    {
        Task<PaymentCardModel?> GetPaymentCardById(Guid paymentCardId);
        Task<IEnumerable<PaymentCardModel>?> GetPaymentCardsByUserId(Guid userId);
    }
}
