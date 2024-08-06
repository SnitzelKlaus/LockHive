using PasswordManager.Users.Domain.Operations;

namespace PasswordManager.Users.ApplicationServices.Repositories.Operations;
/// <summary>
/// Interface for the operation repository, extending the base repository for operation entities.
/// </summary>
public interface IOperationRepository : IBaseRepository<Operation>
{
    /// <summary>
    /// Retrieves the operation with the specified request ID.
    /// </summary>
    /// <param name="requestId">The request ID of the operation to retrieve.</param>
    /// <returns>The operation corresponding to the specified request ID, or null if not found.</returns>
    Task<Operation?> GetByRequestId(string requestId);

    /// <summary>
    /// Retrieves all operations associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose operations to retrieve.</param>
    /// <returns>A collection of all operations associated with the specified user ID.</returns>
    Task<ICollection<Operation>> GetUserOperations(Guid userId);
}