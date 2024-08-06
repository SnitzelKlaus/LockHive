using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.UserPassword.UpdateUserPassword;
using PasswordManager.Users.Domain.Operations;
using Rebus.Handlers;
using Users.Messages.UpdateUserPassword;

namespace Users.Worker.Service.UpdateUserPassword
{
    /// <summary>
    /// Handles the command to update a user's password.
    /// </summary>
    /// <remarks>
    /// This handler takes an <see cref="UpdateUserPasswordCommand"/>, processes it by validating
    /// the associated operation, mapping it to an update model, and then executing the password update. 
    /// It handles logging and operation status updates throughout the process.
    /// </remarks>
    public class UpdateUserPasswordCommandHandler : IHandleMessages<UpdateUserPasswordCommand>
    {
        private readonly IUpdateUserPasswordService _updateUserPasswordService;
        private readonly IOperationService _operationService;
        private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserPasswordCommandHandler"/> class.
        /// </summary>
        /// <param name="updateUserPasswordService">The service to update the user's password.</param>
        /// <param name="operationService">The service for operation status management.</param>
        /// <param name="logger">The logger for recording process information.</param>
        public UpdateUserPasswordCommandHandler(IUpdateUserPasswordService updateUserPasswordService, IOperationService operationService, ILogger<UpdateUserPasswordCommandHandler> logger)
        {
            _updateUserPasswordService = updateUserPasswordService;
            _operationService = operationService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the command to update a user's password
        /// </summary>
        /// <param name="message">The <see cref="UpdateUserPasswordCommand"/> containing the details for the password update operation.</param>
        /// <exception cref="UpdateUserPasswordServiceException">Thrown when the password update fails due to service-related issues.</exception>
        public async Task Handle(UpdateUserPasswordCommand message)
        {
            _logger.LogInformation("Handling update user password command {requestId}", message.RequestId);

            var operation = await _operationService.GetOperationByRequestId(message.RequestId);

            if (operation == null)
            {
                _logger.LogWarning("Operation not found for requestId {requestId}", message.RequestId);
                return;
            }

            await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Processing);
            var updateUserPasswordModel = UpdateUserPasswordOperationHelper.Map(operation.UserId, operation);

            if (updateUserPasswordModel == null)
            {
                _logger.LogWarning("Could not map operation to update user password model for requestId {requestId}", message.RequestId);
                await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Failed);
                return;
            }

            try
            {
                await _updateUserPasswordService.UpdateUserPassword(updateUserPasswordModel);
                await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Completed);
            }
            catch (UpdateUserPasswordServiceException exception)
            {
                _logger.LogError(exception, "Error updating user password with request id: {requestId}", message.RequestId);
                await _operationService.UpdateOperationStatus(operation.RequestId, OperationStatus.Failed);
                return;
            }
        }
    }
}
