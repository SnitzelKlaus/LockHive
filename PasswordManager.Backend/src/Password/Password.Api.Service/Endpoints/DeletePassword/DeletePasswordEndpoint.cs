using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.Api.Service.GetOperation;
using PasswordManager.Password.Api.Service.Models;
using PasswordManager.Password.ApplicationServices.Password.DeletePassword;
using PasswordManager.Password.Domain.Operations;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Password.Api.Service.DeletePassword
{
    public sealed class DeletePasswordEndpoint : EndpointBaseAsync.WithRequest<DeletePasswordRequestWithBody>.WithoutResult
    {
        private readonly IDeletePasswordService _deletePasswordService;

        public DeletePasswordEndpoint(IDeletePasswordService deletePasswordService)
        {
            _deletePasswordService = deletePasswordService;
        }

        [HttpDelete("api/password/{passwordId:guid}")]
        [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        Summary = "Deletes a Password",
        Description = "Deletes a password",
        OperationId = "DeletePassword",
        Tags = new[] { "Password" })
        ]
        public override async Task<ActionResult> HandleAsync([FromQuery] DeletePasswordRequestWithBody request, CancellationToken cancellationToken = default)
        {
            var operationResult = await _deletePasswordService.RequestDeletePassword(request.PasswordId, new OperationDetails(request.CreatedByUserId));

            return operationResult.Status switch
            {
                OperationResultStatus.Accepted => new AcceptedResult(
                    new Uri(GetOperationByRequestIdEndpoint.GetRelativePath(operationResult.GetOperation()), UriKind.Relative),
                    new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),

                OperationResultStatus.InvalidOperationRequest => Problem(title: "Could not delete password", detail: operationResult.GetMessage(),
                    statusCode: StatusCodes.Status400BadRequest),
                _ => Problem(title: "Unknown error requesting to delete password", detail: "Unknown error - check logs",
                    statusCode: StatusCodes.Status500InternalServerError),
            };
        }
    }

    public sealed class DeletePasswordRequestWithBody : OperationRequest
    {
        [FromRoute(Name = "passwordId")]
        public Guid PasswordId { get; set; }
    }
}
