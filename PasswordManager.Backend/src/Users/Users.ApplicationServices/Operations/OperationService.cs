using PasswordManager.Users.ApplicationServices.Repositories.Operations;
using PasswordManager.Users.Domain.Operations;
using Microsoft.Extensions.Logging;

namespace PasswordManager.Users.ApplicationServices.Operations;
/// <summary>
/// Service responsible for managing operations.
/// </summary>
public class OperationService : IOperationService
{
    private readonly ILogger<OperationService> _logger;
    private readonly IOperationRepository _operationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationService"/> class.
    /// </summary>
    /// <param name="logger">The logger for logging messages.</param>
    /// <param name="operationRepository">The repository for accessing operation data.</param>
    public OperationService(ILogger<OperationService> logger, IOperationRepository operationRepository)
    {
        _logger = logger;
        _operationRepository = operationRepository;
    }

    /// <summary>
    /// Queues an operation for processing and stores it in the repository.
    /// </summary>
    /// <param name="operation">The operation to be queued.</param>
    /// <returns>The operation after it has been stored in the repository.</returns>
    public async Task<Operation> QueueOperation(Operation operation)
    {
        if (operation.Status != OperationStatus.Queued)
            throw new ArgumentException("Only pass operations with OperationStatus Queued!");

        var storedOperation = await _operationRepository.Upsert(operation);
        return storedOperation;
    }

    /// <summary>
    /// Retrieves an operation by its request ID.
    /// </summary>
    /// <param name="requestId">The request ID of the operation to retrieve.</param>
    /// <returns>The operation with the specified request ID, or null if not found.</returns>
    public async Task<Operation?> GetOperationByRequestId(string requestId)
    {
        _logger.LogTrace("Getting operation for request id {RequestId}", requestId);

        var operation = await _operationRepository.GetByRequestId(requestId);

        if (operation is not null)
            _logger.LogTrace("Found operation for request id {RequestId}: {OperationName}", requestId, operation.Name);
        else _logger.LogInformation("Could not find operation for request id: {RequestId}", requestId);

        return operation;
    }

    /// <summary>
    /// Updates the status of an operation.
    /// </summary>
    /// <param name="requestId">The request ID of the operation to update.</param>
    /// <param name="operationStatus">The new status of the operation.</param>
    /// <returns>The updated operation after status has been changed.</returns>
    public async Task<Operation?> UpdateOperationStatus(string requestId, OperationStatus operationStatus)
    {
        _logger.LogTrace("Updating operation status for request id {RequestId} to {OperationStatus}", requestId,
            operationStatus);
        var operation = await _operationRepository.GetByRequestId(requestId);
        if (operation is null)
        {
            _logger.LogError(
                "Updating operation status for request id {RequestId} to {OperationStatus} - failed: Could not find operation by request id",
                requestId, operationStatus);
            return null;
        }

        switch (operationStatus)
        {
            case OperationStatus.Processing:
                operation.Processing();
                break;
            case OperationStatus.Completed:
                operation.Complete();
                break;
            case OperationStatus.Failed:
                operation.Failed();
                break;
            case OperationStatus.Queued:
            default:
                throw new ArgumentOutOfRangeException(nameof(operationStatus), operationStatus,
                    "Value was out of range, supported values are:" +
                    $"{nameof(OperationStatus.Completed)}, {nameof(OperationStatus.Processing)}, {nameof(OperationStatus.Failed)}");
        }

        var updatedOperation = await _operationRepository.Upsert(operation);
        _logger.LogTrace("Updated operation status for request id {RequestId} {OperationStatus}", requestId,
            operationStatus);

        return updatedOperation;
    }

    /// <summary>
    /// Retrieves all operations associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose operations to retrieve.</param>
    /// <returns>A collection of operations associated with the specified user.</returns>
    public async Task<ICollection<Operation>> GetUserOperations(Guid UserId)
    {
        _logger.LogTrace("Getting all operations for User: {UserId}", UserId);

        var operations = await _operationRepository.GetUserOperations(UserId);

        _logger.LogTrace("Found {Count} operations for User: {UserId}", operations.Count, UserId);
        return operations;
    }
}
