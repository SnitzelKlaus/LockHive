using PasswordManager.Users.Domain.Operations;

namespace PasswordManager.Users.ApplicationServices.UserPassword.DeleteUserPassword
{
    /// <summary>
    /// Service interface for deleting user passwords.
    /// </summary>
    public interface IDeleteUserPasswordService
    {
        /// <summary>
        /// Requests the deletion of a user password and processes the operation result.
        /// </summary>
        /// <param name="passwordId">The ID of the password to be deleted.</param>
        /// <param name="operationDetails">The details of the operation.</param>
        /// <returns>The result of the password deletion operation.</returns>
        Task<OperationResult> RequestDeleteUserPassword(Guid passwordId, OperationDetails operationDetails);

        /// <summary>
        /// Deletes a user password.
        /// </summary>
        /// <param name="passwordId">The ID of the password to be deleted.</param>
        /// <param name="createdByUserId">The ID of the user who initiated the deletion.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteUserPassword(Guid passwordId, string createdByUserId);
    }
}
