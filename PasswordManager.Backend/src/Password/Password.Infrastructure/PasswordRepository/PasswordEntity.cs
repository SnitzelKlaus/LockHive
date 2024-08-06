using PasswordManager.Password.Infrastructure.BaseRepository;

namespace PasswordManager.Password.Infrastructure.PasswordRepository;

public class PasswordEntity : BaseEntity
{
    public Guid UserId { get; }
    public string Url { get; }
    public string FriendlyName { get; }
    public string Username { get; }
    public string Password { get; }

    public PasswordEntity(Guid id, DateTime createdUtc, DateTime modifiedUtc, Guid userId, string url, string friendlyName, string username, string password)
        : base(id, createdUtc, modifiedUtc)
    {
        UserId = userId;
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
    }
}