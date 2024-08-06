using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.Password.Api.Service.Models
{
    public class PasswordOperationRequest<T> : OperationRequest
    {
        [FromBody] public T Details { get; set; }
    }
}
