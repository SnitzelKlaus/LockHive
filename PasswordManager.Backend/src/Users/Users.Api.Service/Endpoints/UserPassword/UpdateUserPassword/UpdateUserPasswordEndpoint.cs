using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Users.Api.Service.CurrentUser;
using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.ApplicationServices.UserPassword.UpdateUserPassword;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.Api.Service.Endpoints.UserPassword.UpdateUserPassword
{
    /// <summary>
    /// Endpoint for updating a user's password.
    /// </summary>
    public class UpdateUserPasswordEndpoint : EndpointBaseAsync.WithRequest<UpdateUserPasswordRequestWithBody>.WithoutResult
    {
        private readonly IUpdateUserPasswordService _updateUserPasswordService;
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserPasswordEndpoint"/> class.
        /// </summary>
        /// <param name="updateUserPasswordService">The service to update user passwords.</param>
        /// <param name="currentUser">The current user session.</param>
        public UpdateUserPasswordEndpoint(IUpdateUserPasswordService updateUserPasswordService, ICurrentUser currentUser)
        {
            _updateUserPasswordService = updateUserPasswordService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Handles the HTTP PUT request to update a user's password.
        /// </summary>
        /// <param name="request">The request containing the password update information.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation, including the action result.</returns>
        [HttpPut("api/user/passwords/{passwordId}")]
        [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Update a password for a user",
            Description = "Updates a user password",
            OperationId = "UpdateUserPassword",
            Tags = new[] { "Password" })
        ]
        [Authorize(AuthenticationSchemes = "FirebaseUser")]
        public override async Task<ActionResult> HandleAsync([FromRoute] UpdateUserPasswordRequestWithBody request, CancellationToken cancellationToken = default)
        {
            var userPasswordModel = UserPasswordModel.UpdateUserPassword(
                _currentUser.GetUser().Id,
                request.PasswordId,
                request.Details.Url,
                request.Details.FriendlyName,
                request.Details.Username,
                request.Details.Password);

            var operationResult = await _updateUserPasswordService.RequestUpdateUserPassword(userPasswordModel, new OperationDetails(_currentUser.GetUser().Id.ToString()));

            return operationResult.Status switch
            {
                OperationResultStatus.Accepted => Accepted(new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),
                OperationResultStatus.InvalidOperationRequest => Problem(title: "Cannot update user password", detail: operationResult.GetMessage(), statusCode: StatusCodes.Status400BadRequest),
                _ => Problem(title: "Unknown error updating user password", detail: "Unknown error - check logs", statusCode: StatusCodes.Status500InternalServerError),
            };
        }
    }

    /// <summary>
    /// The request for updating a user's password with specific details.
    /// </summary>
    public sealed class UpdateUserPasswordRequestWithBody : UserOperationRequest<UpdateUserPasswordRequestDetails>
    {
        [FromRoute(Name = "passwordId")]
        public Guid PasswordId { get; set; }
    }

    /// <summary>
    /// Detailed request parameters for updating a user's password.
    /// </summary>
    [SwaggerSchema(Nullable = false, Required = new[] { "url", "friendlyName", "username", "password" })]
    public sealed class UpdateUserPasswordRequestDetails
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
}