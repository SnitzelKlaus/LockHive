using Microsoft.Extensions.Logging;
using PasswordManager.User.Domain.Operations;
using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.Domain.Operations;
using Rebus.Bus;
using Users.Messages.DeleteUserPassword;

namespace PasswordManager.Users.ApplicationServices.UserPassword.DeleteUserPassword
{
    /// <summary>
    /// Service responsible for deleting user passwords.
    /// </summary>
    public class DeleteUserPasswordService : IDeleteUserPasswordService
    {
        private readonly IOperationService _operationService;
        private readonly IPasswordComponent _passwordComponent;
        private readonly ILogger<DeleteUserPasswordService> _logger;
        private readonly IBus _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserPasswordService"/> class.
        /// </summary>
        /// <param name="operationService">The service for managing operations.</param>
        /// <param name="passwordComponent">The component responsible for password-related operations.</param>
        /// <param name="logger">The logger for logging messages.</param>
        /// <param name="bus">The message bus for sending commands.</param>
        public DeleteUserPasswordService(IOperationService operationService, IPasswordComponent passwordComponent, ILogger<DeleteUserPasswordService> logger, IBus bus)
        {
            _operationService = operationService;
            _passwordComponent = passwordComponent;
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// Requests the deletion of a user password and processes the operation result.
        /// </summary>
        /// <param name="passwordId">The ID of the password to be deleted.</param>
        /// <param name="operationDetails">The details of the operation.</param>
        /// <returns>The result of the password deletion operation.</returns>
        public async Task<OperationResult> RequestDeleteUserPassword(Guid passwordId, OperationDetails operationDetails)
        {
            _logger.LogInformation("Request deleting password for password id {passwordId}, for user {userId}", passwordId, operationDetails.CreatedBy);

            var operation = await _operationService.QueueOperation(OperationBuilder.DeleteUserPassword(passwordId, operationDetails.CreatedBy));

            await _bus.Send(new DeleteUserPasswordCommand(operation.RequestId));

            _logger.LogInformation("Request sent to worker for deleting password: {passwordId} - requestId: {requestId}", passwordId, operation.RequestId);

            return OperationResult.Accepted(operation);
        }

        /// <summary>
        /// Deletes a user password.
        /// </summary>
        /// <param name="passwordId">The ID of the password to be deleted.</param>
        /// <param name="createdByUserId">The ID of the user who initiated the deletion.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteUserPassword(Guid passwordId, string createdByUserId)
        {
            try
            {
                await _passwordComponent.DeleteUserPassword(passwordId, createdByUserId);
            }
            catch (PasswordComponentException exception)
            {
                throw new DeleteUserPasswordServiceException($"Error calling password component to delete password for user {passwordId}", exception);
            }
        }
    }
}
