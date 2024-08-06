namespace Password.Messages.UpdatePassword;

public sealed class UpdatePasswordCommand : AbstractRequestAcceptedCommand
{
    public UpdatePasswordCommand(string requestId) : base(requestId)
    { 
    }
}
