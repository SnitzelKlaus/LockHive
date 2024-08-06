using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.Api.Service.GetOperation;
using PasswordManager.Password.Api.Service.Models;
using PasswordManager.Password.ApplicationServices.Password.CreatePassword;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.CreatePassword;
public class CreatePasswordEndpoint : EndpointBaseAsync.WithRequest<CreatePasswordRequestWithBody>.WithoutResult
{
    private readonly ICreatePasswordService _createPasswordService;

    public CreatePasswordEndpoint(ICreatePasswordService createPasswordService)
    {
        _createPasswordService = createPasswordService;
    }

    [HttpPost("api/password")]
    [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
    Summary = "Create password",
    Description = "Creates a password",
    OperationId = "CreatePassword",
    Tags = new[] { "Password" })
]
    public override async Task<ActionResult> HandleAsync([FromBody] CreatePasswordRequestWithBody request, CancellationToken cancellationToken = default)
    {
        var passwordModel = PasswordModel.CreatePassword(
            request.Details.UserId, 
            request.Details.Url, 
            request.Details.FriendlyName, 
            request.Details.Username, 
            request.Details.Password
            );

        var operationResult = await _createPasswordService.RequestCreatePassword(passwordModel, new OperationDetails(request.CreatedByUserId));

        return operationResult.Status switch
        {
            OperationResultStatus.Accepted => new AcceptedResult(
                new Uri(GetOperationByRequestIdEndpoint.GetRelativePath(operationResult.GetOperation()), UriKind.Relative),
                new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),

            OperationResultStatus.InvalidOperationRequest => Problem(title: "Cannot password user", detail: operationResult.GetMessage(),
                statusCode: StatusCodes.Status400BadRequest),
            _ => Problem(title: "Unknown error requesting to creating password", detail: "Unknown error - check logs",
                statusCode: StatusCodes.Status500InternalServerError),
        };  
    }
}

public sealed class CreatePasswordRequestWithBody : UserOperationRequest<CreatePasswordRequestDetails>
{
}

[SwaggerSchema(Nullable = false, Required = new[] { "url", "friendlyName", "username", "password", "userId" })]
public sealed class CreatePasswordRequestDetails
{
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
}
