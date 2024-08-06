namespace PasswordManager.Users.Domain.User;
/// <summary>
/// Represents a user's password model.
/// </summary>
public sealed class UserPasswordModel
{
    public Guid UserId { get; }
    public Guid PasswordId { get; }
    public string Url { get; }
    public string FriendlyName { get; }
    public string Username { get; }
    public string Password { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordModel"/> class.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="passwordId">The password ID.</param>
    /// <param name="url">The URL associated with the password.</param>
    /// <param name="friendlyName">The friendly name associated with the password.</param>
    /// <param name="username">The username associated with the password.</param>
    /// <param name="password">The password.</param>
    public UserPasswordModel(Guid userId, Guid passwordId, string url, string friendlyName, string username, string password)
    {
        UserId = userId;
        PasswordId = passwordId;
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordModel"/> class.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="url">The URL associated with the password.</param>
    /// <param name="friendlyName">The friendly name associated with the password.</param>
    /// <param name="username">The username associated with the password.</param>
    /// <param name="password">The password.</param>
    public UserPasswordModel(Guid userId, string url, string friendlyName, string username, string password)
    {
        UserId = userId;
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
    }

    /// <summary>
    /// Creates a new user password model.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="url">The URL associated with the password.</param>
    /// <param name="friendlyName">The friendly name associated with the password.</param>
    /// <param name="username">The username associated with the password.</param>
    /// <param name="password">The password.</param>
    /// <returns>A new instance of <see cref="UserPasswordModel"/>.</returns>
    public static UserPasswordModel CreateUserPassword(Guid userId, string url, string friendlyName, string username, string password)
    {
        var userPasswordModel = new UserPasswordModel(userId, url, friendlyName, username, password);

        return userPasswordModel;
    }

    /// <summary>
    /// Updates an existing user password model.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="passwordId">The password ID.</param>
    /// <param name="url">The URL associated with the password.</param>
    /// <param name="friendlyName">The friendly name associated with the password.</param>
    /// <param name="username">The username associated with the password.</param>
    /// <param name="password">The password.</param>
    /// <returns>An updated instance of <see cref="UserPasswordModel"/>.</returns>
    public static UserPasswordModel UpdateUserPassword(Guid userId, Guid passwordId, string url, string friendlyName, string username, string password)
    {
        var userPasswordModel = new UserPasswordModel(userId, passwordId, url, friendlyName, username, password);

        return userPasswordModel;
    }

    /// <summary>
    /// Sets the encrypted password.
    /// </summary>
    /// <param name="encryptedPassword">The encrypted password.</param>
    /// <returns>The encrypted password.</returns>
    public string SetEncryptedPassword(string encryptedPassword)
    {
        return Password = encryptedPassword;
    }
}
