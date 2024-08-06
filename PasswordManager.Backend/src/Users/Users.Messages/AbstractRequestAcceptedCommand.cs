namespace Users.Messages;
/// <summary>
/// Represents an abstract base class for commands that signify the acceptance of a request.
/// This class encapsulates the common property of a request identifier shared across various types of accepted request commands.
/// </summary>
public abstract class AbstractRequestAcceptedCommand
{
    /// <summary>
    /// Gets the unique identifier of the accepted request.
    /// </summary>
    public string RequestId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractRequestAcceptedCommand"/> class with the specified request identifier.
    /// </summary>
    /// <param name="requestId">The unique identifier of the request that has been accepted.</param>
    protected AbstractRequestAcceptedCommand(string requestId)
    {
        RequestId = requestId;
    }
}
