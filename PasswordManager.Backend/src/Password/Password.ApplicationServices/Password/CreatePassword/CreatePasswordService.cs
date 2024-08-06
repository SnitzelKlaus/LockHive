using Microsoft.Extensions.Logging;
using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using Rebus.Bus;
using Password.Messages.CreatePassword;
using PasswordManager.Password.ApplicationServices.Operations;

namespace PasswordManager.Password.ApplicationServices.Password.CreatePassword;
public sealed class CreatePasswordService : ICreatePasswordService
{
    private readonly IOperationService _operationService;
    private readonly IBus _bus;
    private readonly ILogger<CreatePasswordService> _logger;
    private readonly IPasswordRepository _passwordRepository;

    public CreatePasswordService(IOperationService operationService, IBus bus, ILogger<CreatePasswordService> logger, IPasswordRepository passwordRepository)
    {
        _operationService = operationService;
        _bus = bus;
        _logger = logger;
        _passwordRepository = passwordRepository;
    }

    public async Task<OperationResult> RequestCreatePassword(PasswordModel passwordModel, OperationDetails operationDetails)
    {
        _logger.LogInformation($"Request creating password {passwordModel.Id}");

        var operation = await _operationService.QueueOperation(OperationBuilder.CreatePassword(passwordModel, operationDetails.CreatedBy));

        await _bus.Send(new CreatePasswordCommand(operation.RequestId));

        _logger.LogInformation($"Request sent to worker for creating password: {passwordModel.Id} - requestId: {operation.RequestId}");

        return OperationResult.Accepted(operation);
    }

    public async Task CreatePassword(PasswordModel passwordModel)
    {
        _logger.LogInformation($"Creating password: {passwordModel.Id}");
        try
        {
            await _passwordRepository.Add(passwordModel);

            _logger.LogInformation($"Password created: {passwordModel.Id}");

            return;
        }
        catch (Exception exception)
        {
            throw new CreatePasswordServiceException($"Error calling password repository to create password for user {passwordModel.UserId}", exception);
        }
    }
}
