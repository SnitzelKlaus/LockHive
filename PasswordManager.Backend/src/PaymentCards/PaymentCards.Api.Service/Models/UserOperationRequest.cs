using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.PaymentCards.Api.Service.Models
{
    public abstract class UserOperationRequest<T> : OperationRequest
    {
        [FromBody] public T Details { get; set; }
    }
}
