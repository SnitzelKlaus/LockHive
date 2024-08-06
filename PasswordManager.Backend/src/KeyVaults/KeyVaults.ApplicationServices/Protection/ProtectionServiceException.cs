using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.KeyVaults.ApplicationServices.Protection
{
    public class ProtectionServiceException : Exception
    {
        public ProtectionServiceException(string? message) : base(message) { }

        public ProtectionServiceException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
