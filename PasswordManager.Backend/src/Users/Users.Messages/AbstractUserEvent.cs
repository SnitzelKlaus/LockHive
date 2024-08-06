namespace Users.Messages;
/// <summary>
/// Serves as an abstract base class for all user-related events.
/// This class encapsulates common properties that are shared across various types of user events.
/// </summary>
public abstract class AbstractUserEvent
{
    /// <summary>
    /// Gets the unique identifier of the user associated with the event.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the unique identifier of the request that triggered the event.
    /// </summary>
    public string RequestId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractUserEvent"/> class with the specified user identifier and request identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the event.</param>
    /// <param name="requestId">The unique identifier of the request associated with the event.</param>
    public AbstractUserEvent(Guid userId, string requestId)
    {
        UserId = userId;
        RequestId = requestId;
    }
}
