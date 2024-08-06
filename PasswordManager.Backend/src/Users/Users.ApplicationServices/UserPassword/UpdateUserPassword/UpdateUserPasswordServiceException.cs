namespace PasswordManager.Users.ApplicationServices.UserPassword.UpdateUserPassword
{
    /// <summary>
    /// Exception thrown when an error occurs in the UpdateUserPasswordService.
    /// </summary>
    public class UpdateUserPasswordServiceException : Exception
    {
        public UpdateUserPasswordServiceException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
