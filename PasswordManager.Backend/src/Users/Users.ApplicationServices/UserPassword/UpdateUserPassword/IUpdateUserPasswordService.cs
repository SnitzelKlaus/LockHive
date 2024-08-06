using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.UserPassword.UpdateUserPassword
{
    public interface IUpdateUserPasswordService
    {
        Task<OperationResult> RequestUpdateUserPassword(UserPasswordModel userPasswordModel, OperationDetails operationDetails);
        Task UpdateUserPassword(UserPasswordModel userPasswordModel);
    }
}
