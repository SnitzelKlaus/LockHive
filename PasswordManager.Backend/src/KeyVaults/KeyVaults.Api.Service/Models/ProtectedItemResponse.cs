using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.KeyVaults.Api.Service.Models;

[SwaggerSchema(Nullable = false, Required = new[] { "protectedItem" })]
public class ProtectedItemResponse
{
    [JsonPropertyName("protectedItem")]
    public string ProtectedItem { get; set; }

    public ProtectedItemResponse(string protectedItem)
    {
        ProtectedItem = protectedItem;
    }
}
