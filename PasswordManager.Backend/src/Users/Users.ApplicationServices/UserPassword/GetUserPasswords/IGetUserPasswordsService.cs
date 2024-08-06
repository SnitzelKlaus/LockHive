using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.UserPassword.GetUserPasswords;
public interface IGetUserPasswordsService
{
    Task<IEnumerable<UserPasswordModel>> GetUserPasswords(Guid userId);
    Task<IEnumerable<UserPasswordModel>> GetUserPasswordsByUrl(Guid userId, string url);
}
