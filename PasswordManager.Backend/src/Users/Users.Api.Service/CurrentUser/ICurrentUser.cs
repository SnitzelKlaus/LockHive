using PasswordManager.Users.Infrastructure.UserRepository;

namespace PasswordManager.Users.Api.Service.CurrentUser
{
    public interface ICurrentUser
    {
        UserEntity GetUser();
    }
}
