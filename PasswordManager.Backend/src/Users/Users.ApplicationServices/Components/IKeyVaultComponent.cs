using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.Components;
/// <summary>
/// Interface for the key vault component in the application.
/// </summary>
public interface IKeyVaultComponent
{
    /// <summary>
    /// Creates an encrypted password for the specified user password model using the provided secret key.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the user's password details.</param>
    /// <param name="secretKey">The secret key used for encryption.</param>
    /// <returns>A task representing the asynchronous operation, which returns the encrypted password.</returns>
    Task<string> CreateEncryptedPassword(UserPasswordModel userPasswordModel, string secretKey);

    /// <summary>
    /// Decrypts passwords for the specified collection of user password models using the provided secret key.
    /// </summary>
    /// <param name="userPasswordModels">The collection of user password models to decrypt.</param>
    /// <param name="secretKey">The secret key used for decryption.</param>
    /// <returns>A task representing the asynchronous operation, which returns a collection of decrypted user password models.</returns>
    Task<IEnumerable<UserPasswordModel>> DecryptPasswords(IEnumerable<UserPasswordModel> userPasswordModels, string secretKey);
}
