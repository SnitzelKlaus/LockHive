using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Users.Api.Service.Models;
/// <summary>
/// Represents an abstract base class for operation requests.
/// </summary>
public abstract class OperationRequest
{
    //[FromHeader(Name = "created-by-user-id")][Required] public string CreatedByUserId { get; set; }
}
