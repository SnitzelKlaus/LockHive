using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.PaymentCards.Api.Service.Mappers;
using PasswordManager.PaymentCards.Api.Service.Models;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.GetPaymentCard;
using PasswordManager.PaymentCards.Domain.PaymentCards;
using Swashbuckle.AspNetCore.Annotations;

namespace PasswordManager.PaymentCards.Api.Service.Endpoints.GetPaymentCardsByUserId
{
    public class GetPaymentCardByUserIdEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<IEnumerable<PaymentCardResponse>?>
    {
        private readonly IGetPaymentCardService _getPaymentCardService;
        public GetPaymentCardByUserIdEndpoint(IGetPaymentCardService getPaymentCardService)
        {
            _getPaymentCardService = getPaymentCardService;
        }

        [HttpGet("api/paymentcards/user")]
        [ProducesResponseType(typeof(PaymentCardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Get paymentcards by user id",
            Description = "Gets paymentcards from a user id",
            OperationId = "GetPaymentCardsByUserId",
            Tags = new[] { "PaymentCard" })
        ]
        public override async Task<ActionResult<IEnumerable<PaymentCardResponse>?>> HandleAsync([FromQuery] Guid userId, CancellationToken cancellationToken = default)
        {
            var paymentCardModels = await _getPaymentCardService.GetPaymentCardsByUserId(userId);
            var paymentCardResponses = PaymentCardResponseMapper.Map(paymentCardModels);

            return Ok(paymentCardResponses);
        }
    }
}
