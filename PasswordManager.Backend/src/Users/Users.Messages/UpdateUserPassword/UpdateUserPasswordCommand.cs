namespace Users.Messages.UpdateUserPassword
{
    /// <summary>
    /// Represents a command to update a user's password. This command is issued once the request to update the password is accepted.
    /// Inherits from <see cref="AbstractRequestAcceptedCommand"/> to include a unique request identifier.
    /// </summary>
    public class UpdateUserPasswordCommand : AbstractRequestAcceptedCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserPasswordCommand"/> class with the specified request identifier.
        /// </summary>
        /// <param name="requestId">The unique identifier of the request to update a user's password.</param>
        public UpdateUserPasswordCommand(string requestId) : base(requestId)
        {
        }
    }
}
