using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.UpdatePaymentCard
{
    public interface IUpdatePaymentCardService
    {
        Task<OperationResult> RequestUpdatePaymentCard(PaymentCardModel paymentCardModel, OperationDetails operationDetails);
        Task UpdatePaymentCard(PaymentCardModel paymentCardModel);
    }
}
