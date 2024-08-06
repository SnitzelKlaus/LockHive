using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.Components;
/// <summary>
/// Interface for the password component in the application.
/// </summary>
public interface IPasswordComponent
{
    /// <summary>
    /// Creates a user password.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the user's password details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateUserPassword(UserPasswordModel userPasswordModel);

    /// <summary>
    /// Retrieves user passwords associated with a specific URL.
    /// </summary>
    /// <param name="userId">The ID of the user whose passwords to retrieve.</param>
    /// <param name="url">The URL associated with the passwords to retrieve.</param>
    /// <returns>A collection of user passwords associated with the specified URL.</returns>
    Task<IEnumerable<UserPasswordModel>> GetUserPasswordsFromUrl(Guid userId, string url);

    /// <summary>
    /// Retrieves all user passwords associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose passwords to retrieve.</param>
    /// <returns>A collection of user passwords associated with the specified user.</returns>
    Task<IEnumerable<UserPasswordModel>> GetUserPasswords(Guid userId);

    /// <summary>
    /// Deletes a user password.
    /// </summary>
    /// <param name="passwordId">The ID of the password to delete.</param>
    /// <param name="createdByUserId">The ID of the user who created the password.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteUserPassword(Guid passwordId, string createdByUserId);

    /// <summary>
    /// Updates a user password.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the updated user password details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateUserPassword(UserPasswordModel userPasswordModel);

    /// <summary>
    /// Generates a random password of the specified length.
    /// </summary>
    /// <param name="length">The length of the generated password.</param>
    /// <returns>The generated random password.</returns>
    Task<string> GenerateUserPassword(int length);
}
