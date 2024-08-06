using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.ApplicationServices.Password.GetPassword;

public interface IGetPasswordService
{
    Task<PasswordModel?> GetPassword(Guid passwordId);
    Task<IEnumerable<PasswordModel>> GetPasswords();
    Task<IEnumerable<PasswordModel>> GetPasswordsByUserId(Guid userId);
    Task<IEnumerable<PasswordModel>> GetPasswordsByUserIdWithUrl(Guid userId, string url);
}
