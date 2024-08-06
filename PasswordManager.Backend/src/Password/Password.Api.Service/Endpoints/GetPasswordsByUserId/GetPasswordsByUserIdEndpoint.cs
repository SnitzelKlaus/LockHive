using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.Api.Service.Mappers;
using PasswordManager.Password.Api.Service.Models;
using PasswordManager.Password.ApplicationServices.Password.GetPassword;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.GetPasswordsByUserId
{
    public sealed class GetPasswordsByUserIdEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<IEnumerable<PasswordResponse>>
    {
        private readonly IGetPasswordService _getPasswordService;

        public GetPasswordsByUserIdEndpoint(IGetPasswordService getPasswordService)
        {
            _getPasswordService = getPasswordService;
        }


        [HttpGet("api/passwords/user")]
        [ProducesResponseType(typeof(PasswordResponses), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Gets Passwords from UserId",
            Description = "Gets Passwords from UserId",
            OperationId = "GetPasswordsFromUserId",
            Tags = new[] { "Password" })
        ]
        public override async Task<ActionResult<IEnumerable<PasswordResponse>>> HandleAsync([FromQuery] Guid userId, CancellationToken cancellationToken = default)
        {
            var passwordModels = await _getPasswordService.GetPasswordsByUserId(userId);
            var passwordResponses = PasswordResponseMapper.Map(passwordModels);

            return Ok(passwordResponses);
        }
    }
}
