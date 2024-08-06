using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.PaymentCards.Domain.Operations
{
    public class OperationDetails
    {
        public OperationDetails(string createdBy, string? correlationRequestId = null)
        {
            CreatedBy = createdBy;
            CorrelationRequestId = correlationRequestId;
        }

        public string? CorrelationRequestId { get; }
        public string CreatedBy { get; }
    }
}
