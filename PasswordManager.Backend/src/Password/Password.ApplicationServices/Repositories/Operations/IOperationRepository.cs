using PasswordManager.Password.Domain.Operations;

namespace PasswordManager.Password.ApplicationServices.Repositories.Operations;
public interface IOperationRepository : IBaseRepository<Operation>
{
    Task<Operation?> GetByRequestId(string requestId);
    Task<ICollection<Operation>> GetPasswordOperations(Guid passwordId);
}