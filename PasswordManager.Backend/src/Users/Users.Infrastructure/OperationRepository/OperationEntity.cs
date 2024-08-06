using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Infrastructure.BaseRepository;

namespace PasswordManager.Users.Infrastructure.OperationRepository;
public class OperationEntity : BaseEntity
{
    public string RequestId { get; }
    public Guid UserId { get; }
    public string CreatedBy { get; }
    public OperationName OperationName { get; }
    public DateTime? CompletedUtc { get; }
    public OperationStatus Status { get; }
    public string? Data { get; }

    public OperationEntity(Guid id, DateTime createdUtc, DateTime modifiedUtc, string requestId, Guid userId,
       string createdBy, OperationName operationName, OperationStatus status, DateTime? completedUtc, string? data) :
       base(id, createdUtc, modifiedUtc)
    {
        ModifiedUtc = modifiedUtc;
        RequestId = requestId;
        UserId = userId;
        CreatedBy = createdBy;
        OperationName = operationName;
        Status = status;
        CompletedUtc = completedUtc;
        Data = data;
    }
}
