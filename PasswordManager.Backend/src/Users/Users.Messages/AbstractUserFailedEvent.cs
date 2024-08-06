namespace Users.Messages;
/// <summary>
/// Represents an abstract base class for user-related failure events.
/// </summary>
public abstract class AbstractUserFailedEvent : AbstractUserEvent
{
    /// <summary>
    /// Gets the message describing the failure.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractUserFailedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the event.</param>
    /// <param name="requestId">The unique identifier of the request associated with the event.</param>
    /// <param name="message">The message describing the nature of the failure.</param>
    public AbstractUserFailedEvent(Guid userId, string requestId, string message) : base(userId, requestId)
    {
        Message = message;
    }
}
