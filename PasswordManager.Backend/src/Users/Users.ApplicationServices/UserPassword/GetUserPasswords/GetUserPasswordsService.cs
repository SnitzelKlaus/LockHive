using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.UserPassword.GetUserPasswords;
/// <summary>
/// Service responsible for retrieving user passwords.
/// </summary>
public sealed class GetUserPasswordsService : IGetUserPasswordsService
{
    private readonly IPasswordComponent _passwordComponent;
    private readonly IKeyVaultComponent _KeyVaultComponent;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserPasswordsService"/> class.
    /// </summary>
    /// <param name="passwordComponent">The component responsible for password-related operations.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="keyVaultComponent">The component for managing secrets in the key vault.</param>
    public GetUserPasswordsService(IPasswordComponent passwordComponent, IUserRepository userRepository, IKeyVaultComponent keyVaultComponent)
    {
        _passwordComponent = passwordComponent;
        _userRepository = userRepository;
        _KeyVaultComponent = keyVaultComponent;
    }

    /// <summary>
    /// Retrieves all user passwords for the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose passwords are to be retrieved.</param>
    /// <returns>A collection of user password models.</returns>
    public async Task<IEnumerable<UserPasswordModel>> GetUserPasswords(Guid userId)
    {
        var user = await _userRepository.Get(userId) ?? throw new GetUserPasswordsServiceException("Could not find user");

        if (user.IsDeleted())
        {
            throw new GetUserPasswordsServiceException("Cannot get user password because the user is marked as deleted");
        }

        var encryptedPasswords = await _passwordComponent.GetUserPasswords(userId);

        var decryptedPassword = await _KeyVaultComponent.DecryptPasswords(encryptedPasswords, user.SecretKey);

        return decryptedPassword;
    }

    /// <summary>
    /// Retrieves user passwords for the specified user ID and URL.
    /// </summary>
    /// <param name="userId">The ID of the user whose passwords are to be retrieved.</param>
    /// <param name="url">The URL associated with the passwords to be retrieved.</param>
    /// <returns>A collection of user password models.</returns>
    public async Task<IEnumerable<UserPasswordModel>> GetUserPasswordsByUrl(Guid userId, string url)
    {
        var user = await _userRepository.Get(userId) ?? throw new GetUserPasswordsServiceException("Could not find user");

        if (user.IsDeleted())
        {
            throw new GetUserPasswordsServiceException("Cannot get user password because the user is marked as deleted");
        }

        var encryptedPasswords = await _passwordComponent.GetUserPasswordsFromUrl(userId, url);

        var decryptedPassword = await _KeyVaultComponent.DecryptPasswords(encryptedPasswords, user.SecretKey);

        return decryptedPassword;
    }
}
