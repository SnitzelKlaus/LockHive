using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Users.Api.Service.Models;
/// <summary>
/// Represents the response model for user-related data.
/// </summary>
[SwaggerSchema(Nullable = false, Required = new[] { "id" })]
public class UserResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("firebaseId")]
    public string FirebaseId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserResponse"/> class.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="firebaseId">The Firebase ID of the user.</param>
    public UserResponse(Guid id, string firebaseId)
    {
        Id = id;
        FirebaseId = firebaseId;
    }
}


