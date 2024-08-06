using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;

namespace Password.Worker.Service.UpdatePassword;

internal class UpdatePasswordOperationHelper
{
    internal static PasswordModel Map(Guid passwordId, Operation operation)
    {
        return new PasswordModel(passwordId, GetPasswordUrl(operation), GetPasswordLabel(operation), GetPasswordUsername(operation), GetPasswordKey(operation));
    }

    private static string GetPasswordOperationData(Operation operation, string operationDataConstant)
    {
        if (operation.Data is null || operation.Data.TryGetValue(operationDataConstant, out var getPasswordOperationData) is false)
            throw new InvalidOperationException($"Could not find password {operationDataConstant} in operation with request id {operation.RequestId} when update password");

        return getPasswordOperationData;
    }

    private static string GetPasswordUrl(Operation operation)
    {
        return GetPasswordOperationData(operation, OperationDataConstants.NewPasswordUrl);
    }

    private static string GetPasswordLabel(Operation operation)
    {
        return GetPasswordOperationData(operation, OperationDataConstants.NewPasswordFriendlyName);
    }

    private static string GetPasswordUsername(Operation operation)
    {
        return GetPasswordOperationData(operation, OperationDataConstants.NewPasswordUsername);
    }

    private static string GetPasswordKey(Operation operation)
    {
        return GetPasswordOperationData(operation, OperationDataConstants.NewPasswordPassword);
    }
}
