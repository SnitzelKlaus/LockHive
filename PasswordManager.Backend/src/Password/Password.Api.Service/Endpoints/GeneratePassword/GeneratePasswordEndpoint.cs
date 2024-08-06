using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.ApplicationServices.PasswordGenerator;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.GeneratePassword
{
    public sealed class GeneratePasswordEndpoint : EndpointBaseAsync.WithRequest<GeneratePasswordRequest>.WithActionResult<GeneratePasswordResponse>
    {
        private readonly IGenerateSecureKeyService _generateSecureKeyService;

        public GeneratePasswordEndpoint(IGenerateSecureKeyService generatePasswordService)
        {
            _generateSecureKeyService = generatePasswordService;
        }

        [HttpGet("api/password/generate")]
        [ProducesResponseType(typeof(GeneratePasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
        Summary = "Generates Password",
        Description = "Generates a password",
        OperationId = "GeneratePassword",
        Tags = new[] { "Password" })
        ]

        public override async Task<ActionResult<GeneratePasswordResponse>> HandleAsync([FromQuery] GeneratePasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var password = new GeneratePasswordResponse(await _generateSecureKeyService.GenerateKey(request.PasswordLength));
                return Ok(password);
            }
            catch (Exception ex)
            {
                return Problem(title: "Password could not be generated",
                               detail: $"Password could not be generated: {ex.Message}",
                               statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }

    [SwaggerSchema(Nullable = false, Required = new[] { "passwordLength" })]
    public sealed class GeneratePasswordRequest
    {
        [JsonPropertyName("passwordLength")]
        public int PasswordLength { get; set; }
    }
}
