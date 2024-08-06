using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.Api.Service.GetOperation;
using PasswordManager.Password.Api.Service.Models;
using PasswordManager.Password.ApplicationServices.Password.UpdatePassword;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.UpdatePassword;

public sealed class UpdatePasswordEndpoint : EndpointBaseAsync.WithRequest<UpdatePasswordRequestWithBody>.WithActionResult<PasswordResponse>
{
    private readonly IUpdatePasswordService _updatePasswordService;

    public UpdatePasswordEndpoint(IUpdatePasswordService updatePasswordService)
    {
        _updatePasswordService = updatePasswordService;
    }

    [HttpPut("api/password/{passwordId:guid}")]
    [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Update password",
        Description = "Update password details",
        OperationId = "UpdatePassword",
        Tags = new[] { "Password" })
    ]
    public override async Task<ActionResult<PasswordResponse>> HandleAsync([FromRoute] UpdatePasswordRequestWithBody request, CancellationToken cancellationToken = default)
    {
        var updatePasswordModel = PasswordModel.UpdatePassword(
            request.PasswordId,
            request.Details.Url,
            request.Details.FriendlyName,
            request.Details.Username,
            request.Details.Password
            );

        var operationResult = await _updatePasswordService.RequestUpdatePassword(updatePasswordModel, new OperationDetails(request.CreatedByUserId));

        return operationResult.Status switch
        {
            OperationResultStatus.Accepted => new AcceptedResult(
                new Uri(GetOperationByRequestIdEndpoint.GetRelativePath(operationResult.GetOperation()), UriKind.Relative),
                new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),

            OperationResultStatus.InvalidOperationRequest => Problem(title: "Cannot update password", detail: operationResult.GetMessage(),
                statusCode: StatusCodes.Status400BadRequest),
            _ => Problem(title: "Unknown error requesting to update password", detail: "Unknown error - check logs",
                statusCode: StatusCodes.Status500InternalServerError),
        };
    }
}

public sealed class UpdatePasswordRequestWithBody : PasswordOperationRequest<UpdatePasswordRequestDetails> 
{
    [FromRoute(Name = "passwordId")]
    public Guid PasswordId { get; set; }
}

[SwaggerSchema(Nullable = false, Required = new[] { "url", "friendlyName", "username", "password" })]
public sealed class UpdatePasswordRequestDetails
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("friendlyName")]
    public string FriendlyName { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}