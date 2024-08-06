using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
/// <summary>
/// Service interface for creating user passwords.
/// </summary>
public interface ICreateUserPasswordService
{
    /// <summary>
    /// Requests the creation of a user password and processes the operation result.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the user's password details.</param>
    /// <param name="operationDetails">The details of the operation.</param>
    /// <returns>The result of the password creation operation.</returns>
    Task<OperationResult> RequestCreateUserPassword(UserPasswordModel userPasswordModel, OperationDetails operationDetails);

    /// <summary>
    /// Creates a user password.
    /// </summary>
    /// <param name="userPasswordModel">The model containing the user's password details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateUserPassword(UserPasswordModel userPasswordModel);
}
