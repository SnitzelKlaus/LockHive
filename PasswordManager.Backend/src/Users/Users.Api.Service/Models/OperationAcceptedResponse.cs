namespace PasswordManager.Users.Api.Service.Models
{
    /// <summary>
    /// Represents a response indicating that an operation has been accepted.
    /// </summary>
    public record OperationAcceptedResponse
    {
        /// <summary>
        /// Gets the identifier of the request.
        /// </summary>
        public string RequestId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationAcceptedResponse"/> record.
        /// </summary>
        /// <param name="RequestId">The identifier of the request.</param>
        public OperationAcceptedResponse(string RequestId)
        {
            this.RequestId = RequestId;
        }
    }
}