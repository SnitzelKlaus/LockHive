using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.CreatePaymentCard
{
    public interface ICreatePaymentCardService
    {
        Task<OperationResult> RequestCreatePaymentCard(PaymentCardModel paymentCardModel, OperationDetails operationDetails);
        Task CreatePaymentCard(PaymentCardModel paymentCardModel);

    }
}
