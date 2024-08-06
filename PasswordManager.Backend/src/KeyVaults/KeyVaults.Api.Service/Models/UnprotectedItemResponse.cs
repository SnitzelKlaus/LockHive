using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.KeyVaults.Api.Service.Models;

[SwaggerSchema(Nullable = false, Required = new[] { "unprotectedItem", "itemId" })]
public class UnprotectedItemResponse
{
    [JsonPropertyName("unprotectedItem")]
    public string UnprotectedItem { get; set; }

    [JsonPropertyName("itemId")]
    public Guid ItemId { get; set; }

    public UnprotectedItemResponse(string protectedItem, Guid passwordId)
    {
        UnprotectedItem = protectedItem;
        ItemId = passwordId;
    }
}
