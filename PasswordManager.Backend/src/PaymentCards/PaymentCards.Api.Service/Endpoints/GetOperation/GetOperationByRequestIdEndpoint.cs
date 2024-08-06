using PasswordManager.PaymentCards.ApplicationServices.Repositories.Operations;
using PasswordManager.PaymentCards.Domain.Operations;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using PasswordManager.PaymentCards.ApplicationServices.Operations;

namespace PasswordManager.PaymentCards.Api.Service.Endpoints.GetOperation;

public class GetOperationByRequestIdEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult<OperationResponse>
{
    private readonly IOperationService _operationService;

    public GetOperationByRequestIdEndpoint(IOperationService operationService)
    {
        _operationService = operationService;
    }

    [HttpGet("api/operations/")]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get operation by request id",
        Description = "Get operation by request id",
        OperationId = "GetOperationByRequestId",
        Tags = new[] { "Operations" })
    ]
    public override async Task<ActionResult<OperationResponse>> HandleAsync([FromQuery] string requestId,
        CancellationToken cancellationToken = new())
    {
        var operation = await _operationService.GetOperationByRequestId(requestId);
        if (operation is null)
            return Problem(title: "Operation could not be found",
                detail: $"Operation having request id: '{requestId}' not found", statusCode: StatusCodes.Status404NotFound);

        return new ActionResult<OperationResponse>(OperationMapper.ToResponseModel(operation));
    }

    // Used in accepted result for a path directly to the operation to get progress details
    public static string GetRelativePath(Operation operation)
    {
        return $"/api/operations?requestId={operation.RequestId}";
    }
}
