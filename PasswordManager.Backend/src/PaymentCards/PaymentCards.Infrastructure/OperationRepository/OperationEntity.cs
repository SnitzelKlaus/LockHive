using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Infrastructure.BaseRepository;

namespace PasswordManager.PaymentCards.Infrastructure.OperationRepository;
public class OperationEntity : BaseEntity
{
    public string RequestId { get; private set; }
    public Guid PaymentCardId { get; private set; }
    public string CreatedBy { get; private set; }
    public OperationName OperationName { get; private set; }
    public DateTime? CompletedUtc { get; private set; }
    public OperationStatus Status { get; private set; }
    public string? Data { get; private set; }

    public OperationEntity(Guid id,
        DateTime createdUtc,
        DateTime modifiedUtc,
        string requestId,
        Guid paymentCardId,
        string createdBy,
        OperationName operationName,
        OperationStatus status,
        DateTime? completedUtc,
        string? data,
        bool deleted) : base(id, createdUtc, modifiedUtc, deleted)
    {
        ModifiedUtc = modifiedUtc;
        RequestId = requestId;
        PaymentCardId = paymentCardId;
        CreatedBy = createdBy;
        OperationName = operationName;
        Status = status;
        CompletedUtc = completedUtc;
        Data = data;
    }
}
