namespace PasswordManager.Password.Api.Service.Models;

public class PasswordResponses
{
    public IEnumerable<PasswordResponse> PasswordsResponses { get; set; }

    public PasswordResponses(IEnumerable<PasswordResponse> passwordResponses)
    {
        PasswordsResponses = passwordResponses;
    }
}
