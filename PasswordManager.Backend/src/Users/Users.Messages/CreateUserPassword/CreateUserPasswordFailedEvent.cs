namespace Users.Messages.CreateUserPassword;
/// <summary>
/// Represents an event that is published when an attempt to create a user's password fails.
/// This class provides details about the failure, including the user involved, the request identifier, and a failure message.
/// Inherits from <see cref="AbstractUserFailedEvent"/> to include failure message alongside user and request identifiers.
/// </summary>
public class CreateUserPasswordFailedEvent : AbstractUserFailedEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserPasswordFailedEvent"/> class with specified user identifier, request identifier, and failure message.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the failed password creation event.</param>
    /// <param name="requestId">The unique identifier of the request that led to the failed password creation event.</param>
    /// <param name="message">The message describing the reason for the failure.</param>
    public CreateUserPasswordFailedEvent(Guid userId, string requestId, string message) : base(userId, requestId, message)
    {
    }
}
