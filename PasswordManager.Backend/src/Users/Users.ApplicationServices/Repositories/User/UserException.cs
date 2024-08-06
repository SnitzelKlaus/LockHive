namespace PasswordManager.Users.ApplicationServices.Repositories.User;
/// <summary>
/// Exception thrown by user-related repositories.
/// </summary>
internal class UserException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UserException(string? message) : base(message) { }
}
