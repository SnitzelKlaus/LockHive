using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Users.Api.Service.CurrentUser;
using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.ApplicationServices.UserPassword.DeleteUserPassword;
using PasswordManager.Users.Domain.Operations;
using Swashbuckle.AspNetCore.Annotations;

namespace PasswordManager.Users.Api.Service.Endpoints.UserPassword.DeleteUserPassword
{
    /// <summary>
    /// Endpoint for deleting a user's password.
    /// </summary>
    public class DeleteUserPasswordEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithoutResult
    {
        private readonly IDeleteUserPasswordService _deleteUserPasswordService;
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserPasswordEndpoint"/> class.
        /// </summary>
        /// <param name="deleteUserPasswordService">The service used to delete user passwords.</param>
        /// <param name="currentUser">The current user session.</param>
        public DeleteUserPasswordEndpoint(IDeleteUserPasswordService deleteUserPasswordService, ICurrentUser currentUser)
        {
            _deleteUserPasswordService = deleteUserPasswordService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Handles the HTTP DELETE request to delete a user's password.
        /// </summary>
        /// <param name="passwordId">The unique identifier of the user's password to be deleted.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation, including the action result.</returns>
        [HttpDelete("api/user/password/{passwordId}")]
        [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete a user's password",
            Description = "Deletes a specified user password",
            OperationId = "DeleteUserPassword",
            Tags = new[] { "Password" })
        ]

        [Authorize(AuthenticationSchemes = "FirebaseUser")]
        public override async Task<ActionResult> HandleAsync([FromRoute] Guid passwordId, CancellationToken cancellationToken = default)
        {
            var operationDetails = new OperationDetails(_currentUser.GetUser().Id.ToString());

            var operationResult = await _deleteUserPasswordService.RequestDeleteUserPassword(passwordId, operationDetails);

            return operationResult.Status switch
            {
                OperationResultStatus.Accepted => Accepted(new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),
                OperationResultStatus.InvalidOperationRequest => Problem(title: "Cannot delete user password", detail: operationResult.GetMessage(), statusCode: StatusCodes.Status400BadRequest),
                _ => Problem(title: "Unknown error deleting user password", detail: "Unknown error - check logs", statusCode: StatusCodes.Status500InternalServerError),
            };
        }
    }
}
