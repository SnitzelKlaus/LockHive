using PasswordManager.PaymentCards.ApplicationServices.Repositories.Operations;
using PasswordManager.PaymentCards.Domain.Operations;
using Microsoft.Extensions.Logging;

namespace PasswordManager.PaymentCards.ApplicationServices.Operations;
internal class OperationService : IOperationService
{
    private readonly ILogger<OperationService> _logger;
    private readonly IOperationRepository _operationRepository;

    public OperationService(ILogger<OperationService> logger, IOperationRepository operationRepository)
    {
        _logger = logger;
        _operationRepository = operationRepository;
    }

    public async Task<Operation> QueueOperation(Operation operation)
    {
        if (operation.Status != OperationStatus.Queued)
            throw new ArgumentException("Only pass operations with OperationStatus Queued!");

        var storedOperation = await _operationRepository.Upsert(operation);
        return storedOperation;
    }

    public async Task<Operation?> GetOperationByRequestId(string requestId)
    {
        _logger.LogTrace("Getting operation for request id {RequestId}", requestId);

        var operation = await _operationRepository.GetByRequestId(requestId);

        if (operation is not null)
            _logger.LogTrace("Found operation for request id {RequestId}: {OperationName}", requestId, operation.Name);
        else _logger.LogInformation("Could not find operation for request id: {RequestId}", requestId);

        return operation;
    }

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

    public async Task<ICollection<Operation>> GetPaymentCardOperations(Guid PaymentCardId)
    {
        _logger.LogTrace("Getting all operations for PaymentCard: {PaymentCardId}", PaymentCardId);

        var operations = await _operationRepository.GetPaymentCardOperations(PaymentCardId);

        _logger.LogTrace("Found {Count} operations for PaymentCard: {PaymentCardId}", operations.Count, PaymentCardId);
        return operations;
    }
}
