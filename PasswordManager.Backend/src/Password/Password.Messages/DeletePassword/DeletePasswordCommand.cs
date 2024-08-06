using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password.Messages.DeletePassword
{
    public class DeletePasswordCommand : AbstractRequestAcceptedCommand
    {
        public DeletePasswordCommand(string requestId) : base(requestId)
        {
        }
    }
}
