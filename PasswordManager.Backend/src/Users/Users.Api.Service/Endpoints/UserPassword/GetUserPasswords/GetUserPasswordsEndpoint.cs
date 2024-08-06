using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using PasswordManager.Users.Api.Service.CurrentUser;
using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.Api.Service.Mappers;
using PasswordManager.Users.Domain.User;
using System.ComponentModel.DataAnnotations;
using PasswordManager.Users.ApplicationServices.UserPassword.GetUserPasswords;

namespace PasswordManager.Users.Api.Service.Endpoints.UserPassword.GetUserPasswords;

/// <summary>
/// Endpoint for retrieving a list of user passwords.
/// </summary>
public class GetUserPasswordsEndpoint : EndpointBaseAsync.WithRequest<GetUserPasswordRequestWithBody>.WithActionResult<IEnumerable<UserPasswordResponse>>
{
    private readonly IGetUserPasswordsService _getUserPasswordsService;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserPasswordsEndpoint"/> class.
    /// </summary>
    /// <param name="getUserPasswordsService">The service to get user passwords.</param>
    /// <param name="currentUser">The current user session.</param>
    public GetUserPasswordsEndpoint(IGetUserPasswordsService getUserPasswordsService, ICurrentUser _currentUser)
    {
        _getUserPasswordsService = getUserPasswordsService;
        this._currentUser = _currentUser;
    }

    /// <summary>
    /// Handles the HTTP GET request to retrieve user passwords. It can optionally filter passwords by URL.
    /// </summary>
    /// <param name="request">The request containing optional URL for filtering.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, including the action result with a collection of user passwords.</returns>
    [HttpGet("api/user/passwords")]
    [ProducesResponseType(typeof(IEnumerable<UserPasswordResponse>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get user passwords by user id and url",
        Description = "Get user passwords by url",
        OperationId = "GetUserPasswordsByUrl",
        Tags = new[] { "Password" })
        ]
    [Authorize(AuthenticationSchemes = "FirebaseUser")]
    public override async Task<ActionResult<IEnumerable<UserPasswordResponse>>> HandleAsync([FromQuery] GetUserPasswordRequestWithBody request, CancellationToken cancellationToken = default)
    {
        IEnumerable<UserPasswordModel> userPasswordModel;

        if (request.Url == null)
            userPasswordModel = await _getUserPasswordsService.GetUserPasswords(_currentUser.GetUser().Id);
        else
            userPasswordModel = await _getUserPasswordsService.GetUserPasswordsByUrl(_currentUser.GetUser().Id, request.Url);

        var userPasswordResponse = userPasswordModel.Select(UserPasswordResponseMapper.Map);

        return Ok(userPasswordResponse);
    }
}

/// <summary>
/// The request for retrieving user passwords, with an optional URL for filtering.
/// </summary>
public sealed class GetUserPasswordRequestWithBody
{
    [FromQuery]
    public string? Url { get; set; }
}
