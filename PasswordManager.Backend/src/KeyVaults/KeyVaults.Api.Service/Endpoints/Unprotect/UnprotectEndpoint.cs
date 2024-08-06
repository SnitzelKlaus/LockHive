using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.KeyVaults.Api.Service.Models;
using PasswordManager.KeyVaults.ApplicationServices.Protection;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.KeyVaults.Api.Service.Endpoints.Unprotect
{
    public class UnprotectEndpoint : EndpointBaseAsync.WithRequest<UnprotectItemRequestDetails>.WithActionResult<ProtectedItemsResponse>
    {
        private readonly IProtectionService _protectionService;

        public UnprotectEndpoint(IProtectionService protectionService)
        {
            _protectionService = protectionService;
        }

        [HttpPost("api/keyvaults/unprotect")]
        [ProducesResponseType(typeof(ProtectedItemsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        Summary = "Unprotects item",
        Description = "Unprotects item from received objectId and protectedItem",
        OperationId = "UnprotectItem",
        Tags = new[] { "KeyVault" })
        ]
        public override async Task<ActionResult<ProtectedItemsResponse>> HandleAsync([FromBody] UnprotectItemRequestDetails request, CancellationToken cancellationToken = default)
        {
            try
            {
                List<UnprotectedItemResponse> items = new List<UnprotectedItemResponse>();
                foreach (var item in request.Items)
                {
                    var decryptedItem = _protectionService.Unprotect(item.Key, request.SecretKey);
                    var protectedItemResponse = new UnprotectedItemResponse(decryptedItem, item.Id);

                    items.Add(protectedItemResponse);
                }

                return new ActionResult<ProtectedItemsResponse>(new ProtectedItemsResponse(items));
            }
            catch (ProtectionServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred: Could not unprotect requested item.");
            }
        }
    }

    [SwaggerSchema(Nullable = false, Required = new[] { "secretKey", "items" })]
    public sealed class UnprotectItemRequestDetails
    {
        [JsonPropertyName("secretKey")]
        public string SecretKey { get; set; }

        [JsonPropertyName("items")]
        public IEnumerable<Item> Items { get; set; }
    }

    public sealed class Item
    {
        public string Key { get; }
        public Guid Id { get; }
        public Item(string key, Guid id)
        {
            Key = key;
            Id = id;
        }
    }
}
