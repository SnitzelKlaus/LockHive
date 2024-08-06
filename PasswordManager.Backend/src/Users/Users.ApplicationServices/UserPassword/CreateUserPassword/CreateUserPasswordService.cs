using PasswordManager.User.Domain.Operations;
using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;
using Rebus.Bus;
using Users.Messages.CreateUserPassword;

namespace PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
/// <summary>
/// Service responsible for creating user passwords.
/// </summary>
public class CreateUserPasswordService : ICreateUserPasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly IOperationService _operationService;
    private readonly IBus _bus;
    private readonly IPasswordComponent _passwordComponent;
    private readonly IKeyVaultComponent _keyVaultComponent;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserPasswordService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="operationService">The service for managing operations.</param>
    /// <param name="bus">The message bus for sending commands.</param>
    /// <param name="passwordComponent">The component responsible for password-related operations.</param>
    /// <param name="keyVaultComponent">The component for managing secrets in the key vault.</param>
    public CreateUserPasswordService(IUserRepository userRepository, IOperationService operationService, IBus bus, IPasswordComponent passwordComponent, IKeyVaultComponent keyVaultComponent)
    {
        _userRepository = userRepository;
        _operationService = operationService;
        _bus = bus;
        _passwordComponent = passwordComponent;
        _keyVaultComponent = keyVaultComponent;
    }

    /// <summary>
    /// Requests the creation of a user password and returns the operation result.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the user's password details.</param>
    /// <param name="operationDetails">The details of the operation.</param>
    /// <returns>The result of the password creation operation.</returns>
    public async Task<OperationResult> RequestCreateUserPassword(UserPasswordModel userPasswordModel, OperationDetails operationDetails)
    {
        // Log the request to create the user password.
        var user = await _userRepository.Get(userPasswordModel.UserId);

        // Check if the user was found.
        if (user is null)
        {
            return OperationResult.InvalidState("Cannot create password for user because user was not found");
        }

        // Check if the user was marked as deleted.
        if (user.IsDeleted())
        {
            return OperationResult.InvalidState("Cannot create user password because user was marked as deleted");
        }

        // Encrypts the user password.
        try
        {
            // Encrypt the user password.
            var encryptedPassword = await _keyVaultComponent.CreateEncryptedPassword(userPasswordModel, user.SecretKey);

            if (string.IsNullOrEmpty(encryptedPassword))
            {
                return OperationResult.InvalidState("Cannot create encrypted password for user");
            }

            // Set the encrypted password in the user password model.
            userPasswordModel.SetEncryptedPassword(encryptedPassword);
        }
        catch (KeyVaultComponentException exception)
        {
            return OperationResult.InvalidState(exception.Message);
        }

        // Queue the operation to create the user password.
        var operation = await _operationService.QueueOperation(OperationBuilder.CreateUserPassword(userPasswordModel, operationDetails.CreatedBy));

        // Send a command to bus worker service to create the user password.
        await _bus.Send(new CreateUserPasswordCommand(operation.RequestId));

        // Log the request to create the user password.
        return OperationResult.Accepted(operation);
    }

    /// <summary>
    /// Creates a user password.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the user's password details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateUserPassword(UserPasswordModel userPasswordModel)
    {
        try
        {
            await _passwordComponent.CreateUserPassword(userPasswordModel);
        }
        catch (PasswordComponentException exception)
        {
            throw new CreateUserPasswordServiceException($"Error calling password service to request password for user {userPasswordModel.UserId}", exception);
        }
    }
}
