using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.ApplicationServices.Password.CreatePassword;

public interface ICreatePasswordService
{
    Task<OperationResult> RequestCreatePassword(PasswordModel passwordModel, OperationDetails operationDetails);
    Task CreatePassword(PasswordModel passwordModel);
}
