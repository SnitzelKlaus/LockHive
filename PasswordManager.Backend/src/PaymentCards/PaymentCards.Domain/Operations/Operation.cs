﻿namespace PasswordManager.PaymentCards.Domain.Operations;
public class Operation : BaseModel
{
    public string RequestId { get; private set; }
    public Guid PaymentCardId { get; }
    public string CreatedBy { get; }
    public OperationName Name { get; }
    public OperationStatus Status { get; private set; }
    public DateTime? CompletedUtc { get; private set; }
    public Dictionary<string, string>? Data { get; private set; }

    public Operation(Guid id, string requestId, string createdBy, Guid paymentcardId, OperationName name,
       OperationStatus status, DateTime createdUtc, DateTime modifiedUtc, DateTime? completedUtc,
       Dictionary<string, string>? data) : base(id)
    {
        RequestId = requestId;
        CreatedBy = createdBy;
        PaymentCardId = paymentcardId;
        Name = name;
        Status = status;
        CreatedUtc = createdUtc;
        ModifiedUtc = modifiedUtc;
        CompletedUtc = completedUtc;
        Data = data;
    }

    public Operation(string createdBy, Guid paymentcardId, OperationName name, OperationStatus status)
    {
        Id = Guid.NewGuid();
        RequestId = Guid.NewGuid().ToString();
        CreatedBy = createdBy;
        PaymentCardId = paymentcardId;
        Name = name;
        Status = status;
        CreatedUtc = DateTime.UtcNow;
        ModifiedUtc = DateTime.UtcNow;
    }

    public void Processing()
    {
        Status = OperationStatus.Processing;
    }

    public void Complete()
    {
        CompletedUtc = DateTime.UtcNow;
        Status = OperationStatus.Completed;
    }

    public void Failed()
    {
        CompletedUtc = DateTime.UtcNow;
        Status = OperationStatus.Failed;
    }

    public void OverrideRequestId(string requestId)
    {
        RequestId = requestId;
    }

    public void AddData(string key, string value)
    {
        if (Data is null) Data = new Dictionary<string, string>();
        Data.Add(key, value);
    }
}
