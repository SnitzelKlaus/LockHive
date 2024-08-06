using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.Models;
[SwaggerSchema(Nullable = false, Required = new[] { "id", "url", "friendlyName", "username", "password", "userId" })]
public class PasswordResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("friendlyName")]
    public string FriendlyName { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    public PasswordResponse(Guid id, string url, string friendlyName, string username, string password, Guid userId)
    {
        Id = id;
        Url = url;
        FriendlyName = friendlyName;
        Username = username;
        Password = password;
        UserId = userId;
    }
}


