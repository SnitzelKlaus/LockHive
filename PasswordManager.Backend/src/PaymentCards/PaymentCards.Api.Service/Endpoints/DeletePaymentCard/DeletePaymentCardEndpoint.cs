using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.PaymentCards.Api.Service.Endpoints.GetOperation;
using PasswordManager.PaymentCards.Api.Service.Models;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.DeletePaymentCard;
using PasswordManager.PaymentCards.Domain.Operations;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.PaymentCards.Api.Service.Endpoints.DeletePaymentCard
{
    public class DeletePaymentCardEndpoint : EndpointBaseAsync.WithRequest<DeletePaymentCardRequestBody>.WithoutResult
    {
        private readonly IDeletePaymentCardService _deletePaymentCardService;

        public DeletePaymentCardEndpoint(IDeletePaymentCardService deletePaymentCardService)
        {
            _deletePaymentCardService = deletePaymentCardService;
        }

        [HttpDelete("api/paymentcard/{paymentCardId:guid}")]
        public override async Task<ActionResult> HandleAsync([FromRoute] DeletePaymentCardRequestBody request, CancellationToken cancellationToken = default)
        {
            var operationResult = await _deletePaymentCardService.RequestDeletePaymentCard(request.PaymentCardId, new OperationDetails(request.CreatedByUserId));

            return operationResult.Status switch
            {
                OperationResultStatus.Accepted => new AcceptedResult(
                    new Uri(GetOperationByRequestIdEndpoint.GetRelativePath(operationResult.GetOperation()), UriKind.Relative),
                    new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),

                OperationResultStatus.InvalidOperationRequest => Problem(title: "Could not delete paymentcard", detail: operationResult.GetMessage(),
                    statusCode: StatusCodes.Status400BadRequest),
                _ => Problem(title: "Unknown error requesting to delete paymentcard", detail: "Unknown error - check logs",
                    statusCode: StatusCodes.Status500InternalServerError),
            };
        }
    }

    public sealed class  DeletePaymentCardRequestBody : OperationRequest
    {
        [FromRoute(Name = "paymentCardId")]
        public Guid PaymentCardId { get; set; }
    }
}
