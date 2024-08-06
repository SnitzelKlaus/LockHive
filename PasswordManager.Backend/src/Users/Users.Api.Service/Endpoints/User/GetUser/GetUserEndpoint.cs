using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Users.Api.Service.Mappers;
using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.ApplicationServices.User.GetUser;
using Swashbuckle.AspNetCore.Annotations;

namespace PasswordManager.Users.Api.Service.Endpoints.User.GetUser;

/// <summary>
/// Endpoint for retrieving a user's information by their unique identifier (user ID).
/// </summary>
public class GetUserEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<UserResponse>
{
    private readonly IGetUserService _getUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserEndpoint"/> class.
    /// </summary>
    /// <param name="getUserService">The service responsible for retrieving user data.</param>
    public GetUserEndpoint(IGetUserService getUserService)
    {
        _getUserService = getUserService;
    }

    /// <summary>
    /// Handles the HTTP GET request to retrieve a user by their ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, which upon completion, returns the user's information if found, or an error response if not.</returns>
    [HttpGet("api/user/{userId:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get User by User id",
        Description = "Get User by User id",
        OperationId = "GetUser",
        Tags = new[] { "User" })
    ]
    [Authorize(AuthenticationSchemes = "FirebaseUser")]
    public override async Task<ActionResult<UserResponse>> HandleAsync([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        var passwordModel = await _getUserService.GetUser(userId);

        if (passwordModel is null)
            return Problem(title: "User could not be found",
                           detail: $"User having id: '{userId}' not found",
                           statusCode: StatusCodes.Status404NotFound);

        return new ActionResult<UserResponse>(UserResponseMapper.Map(passwordModel));
    }
}
