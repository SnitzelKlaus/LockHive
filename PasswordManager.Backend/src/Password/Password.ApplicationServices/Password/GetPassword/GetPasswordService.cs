using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.ApplicationServices.Password.GetPassword;

public class GetPasswordService : IGetPasswordService
{
    private readonly IPasswordRepository _passwordRepository;

    public GetPasswordService(IPasswordRepository passwordRepository)
    {
        _passwordRepository = passwordRepository;
    }

    public async Task<PasswordModel?> GetPassword(Guid passwordId)
    {
        return await _passwordRepository.Get(passwordId);
    }

    public async Task<IEnumerable<PasswordModel>> GetPasswords()
    {
        return await _passwordRepository.GetAll();
    }

    public async Task<IEnumerable<PasswordModel>> GetPasswordsByUserId(Guid userId)
    {
        return await _passwordRepository.GetPasswordsByUserId(userId);
    }

    public async Task<IEnumerable<PasswordModel>> GetPasswordsByUserIdWithUrl(Guid userId, string url)
    {
        return await _passwordRepository.GetUserPasswordsWithUrl(userId, url);
    }
}
