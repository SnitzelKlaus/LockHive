using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Users.Api.Service.Endpoints.GetOperation;
using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using PasswordManager.Users.Api.Service.CurrentUser;
using PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
using Microsoft.AspNetCore.Authorization;

namespace PasswordManager.Users.Api.Service.Endpoints.UserPassword.CreateUserPassword;

/// <summary>
/// Endpoint for creating a user's password.
/// </summary>
public class CreateUserPasswordEndpoint : EndpointBaseAsync.WithRequest<CreateUserPasswordRequestWithBody>.WithoutResult
{
    private readonly ICreateUserPasswordService _createUserPasswordService;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserPasswordEndpoint"/> class.
    /// </summary>
    /// <param name="createUserPasswordService">The service to create user passwords.</param>
    /// <param name="currentUser">The current user session.</param>
    public CreateUserPasswordEndpoint(ICreateUserPasswordService createUserPasswordService, ICurrentUser currentUser)
    {
        _createUserPasswordService = createUserPasswordService;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Handles the HTTP POST request to create a new user's password.
    /// </summary>
    /// <param name="request">The request containing the password creation information.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, including the action result.</returns>
    [HttpPost("api/user/passwords")]
    [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
    Summary = "Create a password for a user",
    Description = "Creates a user password",
    OperationId = "CreateUserPassword",
    Tags = new[] { "Password" })
    ]

    [Authorize(AuthenticationSchemes = "FirebaseUser")]
    public override async Task<ActionResult> HandleAsync([FromRoute] CreateUserPasswordRequestWithBody request, CancellationToken cancellationToken = default)
    {
        var userPasswordModel = UserPasswordModel.CreateUserPassword(_currentUser.GetUser().Id, request.Details.Url, request.Details.FriendlyName, request.Details.Username, request.Details.Password);

        var operationResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModel, new OperationDetails(_currentUser.GetUser().Id.ToString()));

        return operationResult.Status switch
        {
            OperationResultStatus.Accepted => new AcceptedResult(
                new Uri(GetOperationByRequestIdEndpoint.GetRelativePath(operationResult.GetOperation()), UriKind.Relative),
                new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),

            OperationResultStatus.InvalidOperationRequest => Problem(title: "Cannot create user password", detail: operationResult.GetMessage(),
                statusCode: StatusCodes.Status400BadRequest),
            _ => Problem(title: "Unknown error requesting to create user password", detail: "Unknown error - check logs",
                statusCode: StatusCodes.Status500InternalServerError),
        };
    }
}

/// <summary>
/// The request for creating a user's password with detailed parameters.
/// </summary>
public sealed class CreateUserPasswordRequestWithBody : UserOperationRequest<CreateUserPasswordRequestDetails>
{
}

/// <summary>
/// Detailed parameters for the request to create a user's password.
/// </summary>
[SwaggerSchema(Nullable = false, Required = new[] { "url", "friendlyName", "username", "password" })]
public sealed class CreateUserPasswordRequestDetails
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