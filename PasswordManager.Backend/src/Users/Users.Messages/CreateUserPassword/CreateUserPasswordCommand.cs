namespace Users.Messages.CreateUserPassword;
/// <summary>
/// Represents a command issued to create a new password for a user. This command is dispatched when a request to create a user's password has been accepted.
/// Inherits from <see cref="AbstractRequestAcceptedCommand"/> to utilize a unique request identifier for tracking the creation request.
/// </summary>
public class CreateUserPasswordCommand : AbstractRequestAcceptedCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserPasswordCommand"/> class with the specified request identifier.
    /// </summary>
    /// <param name="requestId">The unique identifier of the request to create a new user password.</param>
    public CreateUserPasswordCommand(string requestId) : base(requestId)
    {
    }
}
