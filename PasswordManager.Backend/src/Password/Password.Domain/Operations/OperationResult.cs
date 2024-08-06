using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Password.Domain.Operations
{
    public sealed class OperationResult
    {
        public OperationResultStatus Status { get; }
        private Operation? Operation { get; }
        private string? Message { get; }

        private OperationResult(OperationResultStatus status, Operation? operation, string? message)
        {
            Status = status;
            Operation = operation;
            Message = message;
        }

        public static OperationResult Accepted(Operation operation) => new(OperationResultStatus.Accepted, operation, null);
        public static OperationResult Completed(Operation operation) => new(OperationResultStatus.Completed, operation, null);
        public static OperationResult InvalidState(string message) => new(OperationResultStatus.InvalidOperationRequest, null, message);

        public Operation GetOperation()
        {
            if (Status is not (OperationResultStatus.Accepted or OperationResultStatus.Completed) || Operation is null)
                throw new InvalidOperationException("Can't get operation on not ok result");
            return Operation;
        }

        public string GetMessage()
        {
            if (Status is not OperationResultStatus.InvalidOperationRequest || Message is null)
            {
                throw new InvalidOperationException("Can't get error message on not failed result");
            }

            return Message;
        }
    }

    public enum OperationResultStatus
    {
        Accepted,
        InvalidOperationRequest,
        Completed
    }
}
