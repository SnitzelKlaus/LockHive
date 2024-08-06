using PasswordManager.PaymentCards.Domain.Operations;

namespace PasswordManager.PaymentCards.ApplicationServices.PaymentCard.DeletePaymentCard
{
    public interface IDeletePaymentCardService
    {
        Task<OperationResult> RequestDeletePaymentCard(Guid paymentCardId, OperationDetails operationDetails);
        Task DeletePaymentCard(Guid paymentCardId);
    }
}
