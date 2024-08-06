using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.User.Domain.Operations;
/// <summary>
/// Provides methods to build operations.
/// </summary>
public static class OperationBuilder
{
    private static Operation CreateOperation(Guid userId, OperationName operationName, string createdBy, Dictionary<string, string>? data)
    => new(Guid.NewGuid(), Guid.NewGuid().ToString(), createdBy, userId, operationName, OperationStatus.Queued, DateTime.UtcNow, DateTime.UtcNow, null, data);

    /// <summary>
    /// Creates an operation for creating a user password.
    /// </summary>
    /// <param name="passwordModel">The user password model.</param>
    /// <param name="createdBy">The user who created the operation.</param>
    /// <returns>The created operation.</returns>
    public static Operation CreateUserPassword(UserPasswordModel passwordModel, string createdBy)
    {
        var data = new Dictionary<string, string>()
        {
            { OperationDataConstants.CreateUserPasswordUrl, passwordModel.Url },
            { OperationDataConstants.CreateUserPasswordFriendlyName, passwordModel.FriendlyName },
            { OperationDataConstants.CreateUserPasswordUsername, passwordModel.Username },
            { OperationDataConstants.CreateUserPasswordPassword, passwordModel.Password },
        };

        return CreateOperation(passwordModel.UserId, OperationName.CreateUserPassword, createdBy, data);
    }

    /// <summary>
    /// Creates an operation for updating a user password.
    /// </summary>
    /// <param name="newPassword">The new user password model.</param>
    /// <param name="createdBy">The user who created the operation.</param>
    /// <returns>The created operation.</returns>
    public static Operation UpdateUserPassword(UserPasswordModel newPassword, string createdBy)
    {
        var data = new Dictionary<string, string>()
        {
            { OperationDataConstants.UserPasswordId, newPassword.PasswordId.ToString() },
            { OperationDataConstants.NewUserPasswordUrl, newPassword.Url },
            { OperationDataConstants.NewUserPasswordFriendlyName, newPassword.FriendlyName },
            { OperationDataConstants.NewUserPasswordUsername, newPassword.Username },
            { OperationDataConstants.NewUserPasswordPassword, newPassword.Password },
        };

        return CreateOperation(newPassword.UserId, OperationName.UpdateUserPassword, createdBy, data);
    }

    /// <summary>
    /// Creates an operation for deleting a user password.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="createdBy">The user who created the operation.</param>
    /// <returns>The created operation.</returns>
    public static Operation DeleteUserPassword(Guid userId, string createdBy)
    {
        var data = new Dictionary<string, string>()
        {
            { OperationDataConstants.UserPasswordId, userId.ToString() }
        };

        return CreateOperation(userId, OperationName.DeleteUserPassword, createdBy, data);
    }
}