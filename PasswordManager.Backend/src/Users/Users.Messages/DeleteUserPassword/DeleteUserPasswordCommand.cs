namespace Users.Messages.DeleteUserPassword
{
    /// <summary>
    /// Represents a command to delete a user's password. This command is utilized when a request to delete a password has been accepted.
    /// Inherits from <see cref="AbstractRequestAcceptedCommand"/> to leverage a unique request identifier for tracking and processing.
    /// </summary>
    public class DeleteUserPasswordCommand : AbstractRequestAcceptedCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserPasswordCommand"/> class with the specified request identifier.
        /// </summary>
        /// <param name="requestId">The unique identifier of the request for deleting a user's password.</param>
        public DeleteUserPasswordCommand(string requestId) : base(requestId)
        {
        }
    }
}
