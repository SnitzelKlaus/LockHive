namespace PasswordManager.Password.Domain.Password;
public class PasswordModel : BaseModel
{
    public Guid UserId { get; }
    public Guid SecurityKeyId { get; }
    public string Url { get; }
    public string FriendlyName { get; }
    public string Username { get; }
    public string Password { get; }

    public PasswordModel(Guid id, DateTime createdUtc, DateTime modifiedUtc, bool deleted, Guid userId, string url, string friendlyName, string username, string password) : base(id, createdUtc, modifiedUtc, deleted)
    {
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
        UserId = userId;
    }

    public PasswordModel(Guid id, string url, string friendlyName, string username, string password) 
        : base(id)
    {
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
    }

    public PasswordModel(Guid id, Guid userId, string url, string friendlyName, string username, string password)
       : base(id)
    {
        UserId = userId;
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
    }

    public static PasswordModel CreatePassword(Guid userId, string url, string friendlyName, string username, string password)
    {
        return new PasswordModel(Guid.NewGuid(), userId, url, friendlyName, username, password);
    }

    public static PasswordModel UpdatePassword(Guid id, string url, string friendlyName, string username, string password)
    {
        return new PasswordModel(id, url, friendlyName, username, password);
    }
}