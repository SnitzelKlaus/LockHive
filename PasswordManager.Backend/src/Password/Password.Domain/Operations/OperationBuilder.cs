using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.Domain.Operations;

public static class OperationBuilder
{
    private static Operation CreateOperation(Guid userId, OperationName operationName, string createdBy, Dictionary<string, string>? data)
    => new(Guid.NewGuid(), Guid.NewGuid().ToString(), createdBy, userId, operationName, OperationStatus.Queued, DateTime.UtcNow, DateTime.UtcNow, null, data);

    public static Operation CreatePassword(PasswordModel passwordModel, string createdBy)
    {
        var data = new Dictionary<string, string>()
        {
            { OperationDataConstants.PasswordCreateUserId, passwordModel.UserId.ToString() },
            { OperationDataConstants.PasswordCreateUrl, passwordModel.Url },
            { OperationDataConstants.PasswordCreateFriendlyName, passwordModel.FriendlyName },
            { OperationDataConstants.PasswordCreateUsername, passwordModel.Username },
            { OperationDataConstants.PasswordCreatePassword, passwordModel.Password },
        };

        return CreateOperation(passwordModel.Id, OperationName.CreatePassword, createdBy, data);
    }

    public static Operation UpdatePassword(PasswordModel newPasswordModel, PasswordModel currentPasswordModel, string createdBy)
    {
        var data = new Dictionary<string, string>()
        {
            { OperationDataConstants.CurrentPasswordUrl, currentPasswordModel.Url },
            { OperationDataConstants.CurrentPasswordFriendlyName, currentPasswordModel.FriendlyName },
            { OperationDataConstants.CurrentPasswordUsername, currentPasswordModel.Username },
            { OperationDataConstants.CurrentPasswordPassword, currentPasswordModel.Password},

            { OperationDataConstants.NewPasswordUrl , newPasswordModel.Url },
            { OperationDataConstants.NewPasswordFriendlyName , newPasswordModel.FriendlyName },
            { OperationDataConstants.NewPasswordUsername , newPasswordModel.Username},
            { OperationDataConstants.NewPasswordPassword , newPasswordModel.Password},
        };

        return CreateOperation(newPasswordModel.Id, OperationName.UpdatePassword, createdBy, data);
    }

    public static Operation DeletePassword(PasswordModel passwordModel, string createdBy) 
        => CreateOperation(passwordModel.Id, OperationName.DeletePassword, createdBy, null);
}