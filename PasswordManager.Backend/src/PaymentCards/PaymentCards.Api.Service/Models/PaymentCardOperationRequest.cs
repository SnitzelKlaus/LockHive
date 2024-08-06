using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.PaymentCards.Api.Service.Models;

public class PaymentCardOperationRequest<T> : OperationRequest
{
    [FromBody] public T Details { get; set; }
}
