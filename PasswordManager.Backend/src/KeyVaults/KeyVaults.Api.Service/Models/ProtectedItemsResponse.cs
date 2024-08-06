using System.Text.Json.Serialization;

namespace PasswordManager.KeyVaults.Api.Service.Models;

public class ProtectedItemsResponse
{
    [JsonPropertyName("protectedItems")]
    public IEnumerable<UnprotectedItemResponse> UnprotectedItemsResponse { get; set; }

    public ProtectedItemsResponse(IEnumerable<UnprotectedItemResponse> unProtectedItemsResponse)
    {
        UnprotectedItemsResponse = unProtectedItemsResponse;
    }
}
