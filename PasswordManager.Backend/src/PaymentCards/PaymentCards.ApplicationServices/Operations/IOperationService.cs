using PasswordManager.PaymentCards.Domain.Operations;

namespace PasswordManager.PaymentCards.ApplicationServices.Operations;
public interface IOperationService
{
    Task<Operation> QueueOperation(Operation operation);
    Task<Operation?> GetOperationByRequestId(string requestId);
    Task<Operation?> UpdateOperationStatus(string requestId, OperationStatus operationStatus);
    Task<ICollection<Operation>> GetPaymentCardOperations(Guid UserId);
}
