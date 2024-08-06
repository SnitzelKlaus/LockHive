using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Password.ApplicationServices.Password.DeletePassword
{
    public interface IDeletePasswordService
    {
        Task<OperationResult> RequestDeletePassword(Guid passwordId, OperationDetails operationDetails);
        Task DeletePassword(Guid passwordId);
    }
}
