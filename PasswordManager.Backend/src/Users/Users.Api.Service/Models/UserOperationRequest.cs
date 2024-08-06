using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Users.Api.Service.Models;
/// <summary>
/// Represents an abstract base class for user operation requests.
/// </summary>
/// <typeparam name="T">The type of details associated with the operation request.</typeparam>
public abstract class UserOperationRequest<T> : OperationRequest
{
    [FromBody] public T Details { get; set; }
}

