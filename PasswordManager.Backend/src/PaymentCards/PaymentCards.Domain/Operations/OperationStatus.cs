namespace PasswordManager.PaymentCards.Domain.Operations;
public enum OperationStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}
