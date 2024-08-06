namespace Password.Messages.CreatePassword;
public sealed class CreatePasswordFailedEvent : AbstractPasswordFailedEvent
{
    public CreatePasswordFailedEvent(Guid passwordId, string requestId, string message) : base(passwordId, requestId, message)
    {
    }
}
