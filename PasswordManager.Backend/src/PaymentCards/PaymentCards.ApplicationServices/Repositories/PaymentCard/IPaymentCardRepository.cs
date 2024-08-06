using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
public interface IPaymentCardRepository : IBaseRepository<PaymentCardModel>
{
    Task<IEnumerable<PaymentCardModel>?> GetByUserId(Guid userId);
}
