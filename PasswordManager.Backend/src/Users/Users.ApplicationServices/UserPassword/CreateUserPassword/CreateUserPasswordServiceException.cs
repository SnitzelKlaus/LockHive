namespace PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
/// <summary>
/// Exception thrown by the CreateUserPassword service.
/// </summary>
public class CreateUserPasswordServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserPasswordServiceException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public CreateUserPasswordServiceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
