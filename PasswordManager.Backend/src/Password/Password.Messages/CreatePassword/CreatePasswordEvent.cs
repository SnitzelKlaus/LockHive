namespace Password.Messages.CreatePassword;
public class CreatePasswordEvent : AbstractPasswordEvent
{
    public CreatePasswordEvent(Guid passwordId, string requestId) : base(passwordId, requestId)
    {
    }
}
