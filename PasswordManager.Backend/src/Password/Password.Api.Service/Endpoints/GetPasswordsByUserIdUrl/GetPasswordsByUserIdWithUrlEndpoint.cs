using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.Api.Service.Mappers;
using PasswordManager.Password.Api.Service.Models;
using PasswordManager.Password.ApplicationServices.Password.GetPassword;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.GetPasswordsByUserUrl
{
    public sealed class GetPasswordsByUserIdWithUrlEndpoint : EndpointBaseAsync.WithRequest<GetPasswordByUserIdWithUrlRequest>.WithActionResult<IEnumerable<PasswordResponse>>
    {
        private readonly IGetPasswordService _getPasswordService;

        public GetPasswordsByUserIdWithUrlEndpoint(IGetPasswordService getPasswordService)
        {
            _getPasswordService = getPasswordService;
        }


        [HttpGet("api/passwords/by-user-and-url")]
        [ProducesResponseType(typeof(PasswordResponses), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Gets Passwords by UserId and Url",
            Description = "Retrieves passwords associated with a UserId that match a specified URL",
            OperationId = "GetPasswordsByUserIdAndUrl",
            Tags = new[] { "Password" })
        ]
        public override async Task<ActionResult<IEnumerable<PasswordResponse>>> HandleAsync([FromQuery] GetPasswordByUserIdWithUrlRequest request, CancellationToken cancellationToken = default)
        {
            var passwordModels = await _getPasswordService.GetPasswordsByUserIdWithUrl(request.UserId, request.Url);

            var passwordResponses = PasswordResponseMapper.Map(passwordModels);

            return Ok(passwordResponses);
        }
    }

    [SwaggerSchema(Nullable = false, Required = new[] { "userId", "url" })]
    public sealed class GetPasswordByUserIdWithUrlRequest
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
