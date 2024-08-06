using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.ApplicationServices.Password.UpdatePassword;

public interface IUpdatePasswordService
{
    Task<OperationResult> RequestUpdatePassword(PasswordModel updatePasswordModel, OperationDetails operationDetails);
    Task UpdatePassword(PasswordModel updateUserModel);
}
