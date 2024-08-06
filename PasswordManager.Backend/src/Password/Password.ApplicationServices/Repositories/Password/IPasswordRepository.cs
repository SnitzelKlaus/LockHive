using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.ApplicationServices.Repositories.Password;
public interface IPasswordRepository : IBaseRepository<PasswordModel>
{
    Task UpdatePassword(PasswordModel passwordModel);
    Task<IEnumerable<PasswordModel>> GetPasswordsByUserId(Guid userId);
    Task<IEnumerable<PasswordModel>> GetUserPasswordsWithUrl(Guid userId, string url);
}
