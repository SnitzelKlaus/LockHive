using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.UserPassword.DeleteUserPassword;
using PasswordManager.Users.Domain.Operations;
using Rebus.Bus;
using Rebus.Handlers;
using Users.Messages.DeleteUserPassword;

namespace Users.Worker.Service.DeleteUserPassword
{
    /// <summary>
    /// Handles delete user password commands received by the worker service.
    /// </summary>
    public class DeleteUserPasswordCommandHandler : IHandleMessages<DeleteUserPasswordCommand>
    {
        private readonly IDeleteUserPasswordService _deleteUserPasswordService;
        private readonly IOperationService _operationService;
        private readonly ILogger<DeleteUserPasswordCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserPasswordCommandHandler"/> class.
        /// </summary>
        /// <param name="deleteUserPasswordService">The service responsible for deleting user passwords.</param>
        /// <param name="operationService">The service responsible for managing operations.</param>
        /// <param name="logger">The logger for logging messages.</param>
        public DeleteUserPasswordCommandHandler(IDeleteUserPasswordService deleteUserPasswordService, IOperationService operationService, ILogger<DeleteUserPasswordCommandHandler> logger)
        {
            _deleteUserPasswordService = deleteUserPasswordService;
            _operationService = operationService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the delete user password command by processing the operation and deleting the password.
        /// </summary>
        /// <param name="message">The delete user password command message.</param>
        public async Task Handle(DeleteUserPasswordCommand message)
        {
            _logger.LogInformation("Handling delete user password command: {requestId}", message.RequestId);

            // Get the operation associated with the command
            var operation = await _operationService.GetOperationByRequestId(message.RequestId);

            // If operation not found, log a warning and return
            if (operation == null)
            {
                _logger.LogWarning("Operation not found for requestId: {requestId}", message.RequestId);
                return;
            }

            // Update operation status to indicate processing
            await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Processing);

            // Map the operation to obtain the delete password ID
            var deletePasswordId = DeleteUserPasswordOperationHelper.Map(operation);

            try
            {
                // Attempt to delete the user password
                await _deleteUserPasswordService.DeleteUserPassword(deletePasswordId, operation.CreatedBy);

                // Update operation status to indicate completion
                await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Completed);
            }
            catch (DeleteUserPasswordServiceException exception)
            {
                // Log error if deletion fails and update operation status to indicate failure
                _logger.LogError(exception, "Error deleting user password with request id: {requestId}", message.RequestId);
                await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Failed);
            }
        }
    }
}
