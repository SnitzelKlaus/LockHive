using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;

namespace Users.Worker.Service.CreateUserPassword;
/// <summary>
/// Helper class for mapping data from an operation to a UserPasswordModel when creating a user password.
/// </summary>
public class CreateUserPasswordOperationHelper
{
    /// <summary>
    /// Maps data from an operation to a UserPasswordModel.
    /// </summary>
    /// <param name="userId">The ID of the user for whom the password is being created.</param>
    /// <param name="operation">The operation containing data for creating the user password.</param>
    /// <returns>A UserPasswordModel instance with mapped data.</returns>
    internal static UserPasswordModel Map(Guid userId, Operation operation)
    {
        return new UserPasswordModel(userId, GetPasswordUrl(operation), GetPasswordLabel(operation), GetPasswordUsername(operation), GetPasswordKey(operation));
    }

    private static string GetUserPasswordOperationData(Operation operation, string operationDataConstant)
    {
        if (operation.Data is null || operation.Data.TryGetValue(operationDataConstant, out var getPasswordOperationData) is false)
            throw new InvalidOperationException($"Could not find user password {operationDataConstant} in operation with request id {operation.RequestId} when creating user password");

        return getPasswordOperationData;
    }

    private static string GetPasswordUrl(Operation operation)
    {
        return GetUserPasswordOperationData(operation, OperationDataConstants.CreateUserPasswordUrl);
    }

    private static string GetPasswordLabel(Operation operation)
    {
        return GetUserPasswordOperationData(operation, OperationDataConstants.CreateUserPasswordFriendlyName);
    }

    private static string GetPasswordUsername(Operation operation)
    {
        return GetUserPasswordOperationData(operation, OperationDataConstants.CreateUserPasswordUsername);
    }

    private static string GetPasswordKey(Operation operation)
    {
        return GetUserPasswordOperationData(operation, OperationDataConstants.CreateUserPasswordPassword);
    }
}
