using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.Repositories.User;
/// <summary>
/// Interface for the user repository, extending the base repository for user models.
/// </summary>
public interface IUserRepository : IBaseRepository<UserModel>
{
}
