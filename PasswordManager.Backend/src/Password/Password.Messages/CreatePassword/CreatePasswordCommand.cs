namespace Password.Messages.CreatePassword;

public sealed class CreatePasswordCommand : AbstractRequestAcceptedCommand
{
    public CreatePasswordCommand(string requestId) : base(requestId)
    {
    }
}
