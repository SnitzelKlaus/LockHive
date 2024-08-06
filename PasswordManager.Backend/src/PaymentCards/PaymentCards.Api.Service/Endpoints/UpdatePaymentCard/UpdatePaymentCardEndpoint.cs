using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.PaymentCards.Api.Service.Endpoints.GetOperation;
using PasswordManager.PaymentCards.Api.Service.Mappers;
using PasswordManager.PaymentCards.Api.Service.Models;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.UpdatePaymentCard;
using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.PaymentCards.Api.Service.Endpoints.UpdatePaymentCard
{
    public class UpdatePaymentCardEndpoint : EndpointBaseAsync.WithRequest<UpdatePaymentCardRequestWithBody>.WithActionResult<PaymentCardResponse>
    {
        private readonly IUpdatePaymentCardService _updatePaymentCardService;

        public UpdatePaymentCardEndpoint(IUpdatePaymentCardService updatePaymentCardService)
        {
            _updatePaymentCardService = updatePaymentCardService;
        }

        [HttpPut("api/paymentcard/{paymentCardId:Guid}")]
        [ProducesResponseType(typeof(OperationAcceptedResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        Summary = "Update paymentcard",
        Description = "Updates a paymentcard",
        OperationId = "UpdatePaymentCard",
        Tags = new[] { "PaymentCard" })
        ]
        public override async Task<ActionResult<PaymentCardResponse>> HandleAsync([FromRoute] UpdatePaymentCardRequestWithBody request, CancellationToken cancellationToken = default)
        {
            var updatedPaymentCardModel = PaymentCardModel.UpdatePaymentCard(
                request.PaymentCardId,
                request.Details.CardNumber,
                request.Details.CardHolderName,
                request.Details.ExpiryMonth,
                request.Details.ExpiryYear,
                request.Details.Cvv
                );

            var operationResult = await _updatePaymentCardService.RequestUpdatePaymentCard(updatedPaymentCardModel, new OperationDetails(request.CreatedByUserId));

            return operationResult.Status switch
            {
                OperationResultStatus.Accepted => new AcceptedResult(
                    new Uri(GetOperationByRequestIdEndpoint.GetRelativePath(operationResult.GetOperation()), UriKind.Relative),
                    new OperationAcceptedResponse(operationResult.GetOperation().RequestId)),

                OperationResultStatus.InvalidOperationRequest => Problem(title: "Cannot update paymentcard", detail: operationResult.GetMessage(),
                    statusCode: StatusCodes.Status400BadRequest),
                _ => Problem(title: "Unknown error requesting to update payment", detail: "Unknown error - check logs",
                    statusCode: StatusCodes.Status500InternalServerError),
            };
        }
    }

    public sealed class UpdatePaymentCardRequestWithBody : PaymentCardOperationRequest<UpdatePaymentCardRequestDetails>
    {
        [FromRoute(Name = "paymentCardId")]
        public Guid PaymentCardId { get; set; }
    }

    [SwaggerSchema(Nullable = false, Required = new[] { "cardNumber", "cardHolderName", "expiryMonth", "expiryYear", "cvv" })]
    public sealed class UpdatePaymentCardRequestDetails
    {
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; }

        [JsonPropertyName("cardHolderName")]
        public string CardHolderName { get; set; }

        [JsonPropertyName("expiryMonth")]
        public int ExpiryMonth { get; set; }

        [JsonPropertyName("expiryYear")]
        public int ExpiryYear { get; set; }

        [JsonPropertyName("cvv")]
        public string Cvv { get; set; }
    }
}
