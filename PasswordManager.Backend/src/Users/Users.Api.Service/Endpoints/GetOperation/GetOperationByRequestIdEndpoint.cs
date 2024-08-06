using PasswordManager.Users.Domain.Operations;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using PasswordManager.Users.ApplicationServices.Operations;

namespace PasswordManager.Users.Api.Service.Endpoints.GetOperation;

/// <summary>
/// Endpoint for retrieving the details of an operation by its request ID.
/// </summary>
public class GetOperationByRequestIdEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult<OperationResponse>
{
    private readonly IOperationService _operationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOperationByRequestIdEndpoint"/> class.
    /// </summary>
    /// <param name="operationService">The service to interact with operations.</param>
    public GetOperationByRequestIdEndpoint(IOperationService operationService)
    {
        _operationService = operationService;
    }

    /// <summary>
    /// Handles the HTTP GET request to retrieve an operation by its request ID.
    /// </summary>
    /// <param name="requestId">The request ID of the operation to retrieve.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, including the action result with operation details.</returns>
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

    /// <summary>
    /// Constructs the relative path to an operation for accessing its progress details.
    /// </summary>
    /// <param name="operation">The operation to generate the path for.</param>
    /// <returns>A string representing the relative path to the operation's details.</returns>
    public static string GetRelativePath(Operation operation)
    {
        return $"/api/operations?requestId={operation.RequestId}";
    }
}
