namespace Password.Messages.UpdatePassword;
public class UpdatePasswordFailedEvent : AbstractPasswordFailedEvent
{
    public UpdatePasswordFailedEvent(Guid passwordId, string requestId, string message) : base(passwordId, requestId, message)
    {
    }
}
