namespace Password.Messages;
public abstract class AbstractPasswordEvent
{
    public Guid PasswordId { get; }
    public string RequestId { get; }
    public AbstractPasswordEvent(Guid passwordId, string requestId)
    {
        PasswordId = passwordId;
        RequestId = requestId;
    }
}
