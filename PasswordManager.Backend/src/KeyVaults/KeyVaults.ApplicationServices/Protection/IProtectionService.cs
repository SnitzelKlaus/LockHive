namespace PasswordManager.KeyVaults.ApplicationServices.Protection
{
    public interface IProtectionService
    {
        string Protect(string item, string key);
        string Unprotect(string protectedItem, string key);
    }
}
