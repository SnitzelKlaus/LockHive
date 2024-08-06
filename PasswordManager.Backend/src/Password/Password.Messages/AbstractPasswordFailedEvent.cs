namespace Password.Messages;
public abstract class AbstractPasswordFailedEvent : AbstractPasswordEvent
{
    public string Message { get; }

    protected AbstractPasswordFailedEvent(Guid passwordId, string requestId, string message) : base(passwordId, requestId)
    {
        Message = message;
    }
}
