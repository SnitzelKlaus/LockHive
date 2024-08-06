using PasswordManager.Password.Domain.Operations;

namespace PasswordManager.Password.ApplicationServices.Operations;
public interface IOperationService
{
    Task<Operation> QueueOperation(Operation operation);
    Task<Operation?> GetOperationByRequestId(string requestId);
    Task<Operation?> UpdateOperationStatus(string requestId, OperationStatus operationStatus);
    Task<ICollection<Operation>> GetPasswordOperations(Guid UserId);
}
