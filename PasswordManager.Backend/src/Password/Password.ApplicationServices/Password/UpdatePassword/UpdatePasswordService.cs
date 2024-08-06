using Microsoft.Extensions.Logging;
using Password.Messages.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using Rebus.Bus;

namespace PasswordManager.Password.ApplicationServices.Password.UpdatePassword;

public class UpdatePasswordService : IUpdatePasswordService
{
    private readonly IOperationService _operationService;
    private readonly IBus _bus;
    private readonly ILogger<UpdatePasswordService> _logger;
    private readonly IPasswordRepository _passwordRepository;

    public UpdatePasswordService(IOperationService operationService, IBus bus, ILogger<UpdatePasswordService> logger, IPasswordRepository passwordRepository)
    {
        _operationService = operationService;
        _bus = bus;
        _logger = logger;
        _passwordRepository = passwordRepository;
    }

    public async Task<OperationResult> RequestUpdatePassword(PasswordModel updatePasswordModel, OperationDetails operationDetails)
    {
        _logger.LogInformation("Request updating password {PasswordId}", updatePasswordModel.Id);

        var currentPasswordModel = await _passwordRepository.Get(updatePasswordModel.Id);

        if (currentPasswordModel is null)
        {
            _logger.LogInformation("Cannot update password {PasswordId} because it was not found", updatePasswordModel.Id);

            return OperationResult.InvalidState("Cannot update password because it was not found");
        }

        if (currentPasswordModel.Deleted)
        {
            _logger.LogInformation("Cannot update password {PasswordId} because it was marked as deleted", updatePasswordModel.Id);

            return OperationResult.InvalidState("Cannot update password because it was marked as deleted");
        }

        var operation = await _operationService.QueueOperation(OperationBuilder.UpdatePassword(updatePasswordModel, currentPasswordModel, operationDetails.CreatedBy));

        await _bus.Send(new UpdatePasswordCommand(operation.RequestId));

        _logger.LogInformation("Request sent to worker for creating password: {PasswordId} - RequestId: {RequestId}", updatePasswordModel.Id, operation.RequestId);

        return OperationResult.Accepted(operation);
    }

    public async Task UpdatePassword(PasswordModel updatePasswordModel)
    {
        _logger.LogInformation("Updating password: {updatePasswordModelId}", updatePasswordModel.Id);

        await _passwordRepository.UpdatePassword(updatePasswordModel);

        _logger.LogInformation("Password updated: {updatePasswordModelId}", updatePasswordModel.Id);

        return;
    }
}
