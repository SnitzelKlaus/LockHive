using PasswordManager.Users.Domain.Operations;

namespace PasswordManager.Users.ApplicationServices.Operations;
/// <summary>
/// Service interface for managing operations.
/// </summary>
public interface IOperationService
{
    /// <summary>
    /// Queues an operation for processing and stores it in the repository.
    /// </summary>
    /// <param name="operation">The operation to be queued.</param>
    /// <returns>The operation after it has been stored in the repository.</returns>
    Task<Operation> QueueOperation(Operation operation);

    /// <summary>
    /// Retrieves an operation by its request ID.
    /// </summary>
    /// <param name="requestId">The request ID of the operation to retrieve.</param>
    /// <returns>The operation with the specified request ID, or null if not found.</returns>
    Task<Operation?> GetOperationByRequestId(string requestId);

    /// <summary>
    /// Updates the status of an operation.
    /// </summary>
    /// <param name="requestId">The request ID of the operation to update.</param>
    /// <param name="operationStatus">The new status of the operation.</param>
    /// <returns>The updated operation after status has been changed.</returns>
    Task<Operation?> UpdateOperationStatus(string requestId, OperationStatus operationStatus);

    /// <summary>
    /// Retrieves all operations associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose operations to retrieve.</param>
    /// <returns>A collection of operations associated with the specified user.</returns>
    Task<ICollection<Operation>> GetUserOperations(Guid UserId);
}
