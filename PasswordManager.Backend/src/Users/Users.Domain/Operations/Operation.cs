namespace PasswordManager.Users.Domain.Operations;
/// <summary>
/// Represents an operation.
/// </summary>
public class Operation : BaseModel
{
    public string RequestId { get; private set; }
    public Guid UserId { get; }
    public string CreatedBy { get; }
    public OperationName Name { get; }
    public OperationStatus Status { get; private set; }
    public DateTime? CompletedUtc { get; private set; }
    public Dictionary<string, string>? Data { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Operation"/> class.
    /// </summary>
    public Operation(Guid id, string requestId, string createdBy, Guid userId, OperationName name,
       OperationStatus status, DateTime createdUtc, DateTime modifiedUtc, DateTime? completedUtc,
       Dictionary<string, string>? data) : base(id)
    {
        RequestId = requestId;
        CreatedBy = createdBy;
        UserId = userId;
        Name = name;
        Status = status;
        CreatedUtc = createdUtc;
        ModifiedUtc = modifiedUtc;
        CompletedUtc = completedUtc;
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Operation"/> class with default values.
    /// </summary>
    public Operation(string createdBy, Guid userId, OperationName name, OperationStatus status)
    {
        Id = Guid.NewGuid();
        RequestId = Guid.NewGuid().ToString();
        CreatedBy = createdBy;
        UserId = userId;
        Name = name;
        Status = status;
        CreatedUtc = DateTime.UtcNow;
        ModifiedUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the operation status to "Processing".
    /// </summary>
    public void Processing()
    {
        Status = OperationStatus.Processing;
    }

    /// <summary>
    /// Sets the operation status to "Completed" and updates the completion date.
    /// </summary>
    public void Complete()
    {
        CompletedUtc = DateTime.UtcNow;
        Status = OperationStatus.Completed;
    }

    /// <summary>
    /// Sets the operation status to "Failed" and updates the completion date.
    /// </summary>
    public void Failed()
    {
        CompletedUtc = DateTime.UtcNow;
        Status = OperationStatus.Failed;
    }

    /// <summary>
    /// Overrides the request ID of the operation.
    /// </summary>
    /// <param name="requestId">The new request ID.</param>
    public void OverrideRequestId(string requestId)
    {
        RequestId = requestId;
    }

    /// <summary>
    /// Adds data to the operation.
    /// </summary>
    /// <param name="key">The key of the data.</param>
    /// <param name="value">The value of the data.</param>
    public void AddData(string key, string value)
    {
        if (Data is null) Data = new Dictionary<string, string>();
        Data.Add(key, value);
    }
}
