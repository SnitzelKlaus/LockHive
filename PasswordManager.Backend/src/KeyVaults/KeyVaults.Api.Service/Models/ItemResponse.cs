using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.KeyVaults.Api.Service.Models
{
    [SwaggerSchema(Nullable = false, Required = new[] { "items" })]
    public class ItemResponse
    {
        [JsonPropertyName("items")]
        public IEnumerable<string> Items { get; set; }

        public ItemResponse(IEnumerable<string> items)
        {
            Items = items;
        }
    }
}
