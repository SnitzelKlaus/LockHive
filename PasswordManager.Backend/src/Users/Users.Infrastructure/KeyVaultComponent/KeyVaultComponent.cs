using Microsoft.Extensions.Logging;
using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.Domain.User;
using System.Reflection.Metadata.Ecma335;
using Umbraco.Cloud.Passwordmanager.Keyvaults.Api.Client;

namespace PasswordManager.Users.Infrastructure.KeyVaultComponent;
/// <summary>
/// Handles encryption and decryption of user passwords or paymentcards using a key vault service.
/// </summary>
public sealed class KeyVaultComponent : IKeyVaultComponent
{
    private readonly IPasswordmanagerKeyvaultsApiClient _passwordmanagerKeyvaultsApiClient;
    private ILogger<KeyVaultComponent> _logger;

    /// <summary>
    /// Initializes a new instance of the KeyVaultComponent with the specified API client and logger.
    /// </summary>
    /// <param name="passwordmanagerKeyvaultsApiClient">The API client for interacting with the key vault.</param>
    /// <param name="logger">The logger for logging operations.</param>
    public KeyVaultComponent(IPasswordmanagerKeyvaultsApiClient passwordmanagerKeyvaultsApiClient, ILogger<KeyVaultComponent> logger)
    {
        _passwordmanagerKeyvaultsApiClient = passwordmanagerKeyvaultsApiClient;
        _logger = logger;
    }

    /// <summary>
    /// Encrypts a password using a secret key.
    /// </summary>
    /// <param name="userPasswordModel">The user password model containing the password to encrypt.</param>
    /// <param name="secretKey">The secret key used for encryption.</param>
    /// <returns>The encrypted password.</returns>
    public async Task<string> CreateEncryptedPassword(UserPasswordModel userPasswordModel, string secretKey)
    {
        try
        {
            _logger.LogInformation("Requesting to create password");
            var encryptedPassword = await _passwordmanagerKeyvaultsApiClient.ProtectItemAsync(
                new ProtectItemRequestDetails(userPasswordModel.Password, secretKey));

            var encryptedPasswordResult = encryptedPassword.Result;

            return encryptedPasswordResult.ProtectedItem;
        }
        catch (ApiException exception)
        {
            _logger.LogError(exception, "Could not encrypt password, service returned {StatusCode} and message {ErrorMessage}",
               exception.StatusCode, exception.Message);

            throw new KeyVaultComponentException("Error calling KeyVaultApiClient.ProtectItemAsync", exception);
        }
    }

    /// <summary>
    /// Decrypts a collection of encrypted passwords using a secret key.
    /// </summary>
    /// <param name="userPasswordModels">The collection of user password models to decrypt.</param>
    /// <param name="secretKey">The secret key used for decryption.</param>
    /// <returns>A collection of user password models with decrypted passwords.</returns>
    public async Task<IEnumerable<UserPasswordModel>> DecryptPasswords(IEnumerable<UserPasswordModel> userPasswordModels, string secretKey)
    {
        var items = new List<Item>(userPasswordModels.Select(user => new Item(user.PasswordId, user.Password)));
        var decryptedPasswords = await _passwordmanagerKeyvaultsApiClient.UnprotectItemAsync(new UnprotectItemRequestDetails(items, secretKey));

        var decryptedPasswordsResult = decryptedPasswords.Result;

        List<UserPasswordModel> decryptedUserPasswords = new List<UserPasswordModel>();

        foreach (var user in userPasswordModels)
        {
            foreach (var decryptedPassword in decryptedPasswordsResult.ProtectedItems)
            {
                if (decryptedPassword.ItemId == user.PasswordId)
                {
                    user.SetEncryptedPassword(decryptedPassword.UnprotectedItem);
                }
            }
            decryptedUserPasswords.Add(user);
        }

        return decryptedUserPasswords;
    }
}
