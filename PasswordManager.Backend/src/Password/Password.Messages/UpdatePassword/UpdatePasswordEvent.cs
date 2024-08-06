namespace Password.Messages.UpdatePassword;
public sealed class UpdatePasswordEvent : AbstractPasswordEvent
{
    public UpdatePasswordEvent(Guid passwordId, string requestId) : base(passwordId, requestId)
    {
    }
}
