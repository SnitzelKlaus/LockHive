namespace Users.Messages.CreateUserPassword;
/// <summary>
/// Represents an event that is published when a user's password is successfully created.
/// This class captures essential information such as the user involved and the request identifier, 
/// facilitating tracking and auditing of the password creation process.
/// Inherits from <see cref="AbstractUserEvent"/> to include user and request identifiers.
/// </summary>
public class CreateUserPasswordEvent : AbstractUserEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserPasswordEvent"/> class with the specified user identifier and request identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the password is created.</param>
    /// <param name="requestId">The unique identifier of the request leading to the password creation.</param>
    public CreateUserPasswordEvent(Guid userId, string requestId) : base(userId, requestId)
    {
    }
}
