namespace PasswordManager.Password.ApplicationServices.PasswordGenerator
{
    public interface IGenerateSecureKeyService
    {
        Task<string> GenerateKey(int length);
    }
}
