using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.User.GetUser;
/// <summary>
/// Service interface for retrieving user information.
/// </summary>
public interface IGetUserService
{
    /// <summary>
    /// Retrieves user information based on the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user model corresponding to the specified user ID.</returns>
    Task<UserModel> GetUser(Guid userId);
}