using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Users.Api.Service.Models;
/// <summary>
/// Represents a response model for user passwords.
/// </summary>
[SwaggerSchema(Nullable = false, Required = new[] { "id", "passwordId", "url", "friendlyName", "username", "password" })]
public class UserPasswordResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("passwordId")]
    public Guid PasswordId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("friendlyName")]
    public string FriendlyName { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordResponse"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The ID of the user password.</param>
    /// <param name="passwordId">The password ID.</param>
    /// <param name="url">The URL associated with the password.</param>
    /// <param name="friendlyName">The friendly name associated with the password.</param>
    /// <param name="username">The username associated with the password.</param>
    /// <param name="password">The password.</param>
    public UserPasswordResponse(Guid id, Guid passwordId, string url, string friendlyName, string username, string password)
    {
        Id = id;
        PasswordId = passwordId;
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
    }
}
