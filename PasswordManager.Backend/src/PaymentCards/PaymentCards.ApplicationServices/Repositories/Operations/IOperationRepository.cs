using PasswordManager.PaymentCards.Domain.Operations;

namespace PasswordManager.PaymentCards.ApplicationServices.Repositories.Operations;
public interface IOperationRepository : IBaseRepository<Operation>
{
    Task<Operation?> GetByRequestId(string requestId);
    Task<ICollection<Operation>> GetPaymentCardOperations(Guid paymentcardId);
}