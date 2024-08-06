using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Users.Api.Service.Models;
/// <summary>
/// Represents a generic request model for user-related data.
/// </summary>
/// <typeparam name="T">The type of additional details associated with the request.</typeparam>
public class UserRequest<T> 
{
    [FromRoute(Name = "userId")]
    [Required]
    public Guid UserId { get; set; }
    [FromBody] public T Details { get; set; }
}
