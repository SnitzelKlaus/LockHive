using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.PaymentCards.Api.Service.Mappers;
using PasswordManager.PaymentCards.Api.Service.Models;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.GetPaymentCard;
using Swashbuckle.AspNetCore.Annotations;

namespace PasswordManager.PaymentCards.Api.Service.Endpoints.GetPaymentCardById;

public class GetPaymentCardEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<PaymentCardResponse>
{
    private readonly IGetPaymentCardService _getPaymentCardService;
    public GetPaymentCardEndpoint(IGetPaymentCardService getPaymentCardService)
    {
        _getPaymentCardService = getPaymentCardService;
    }

    [HttpGet("api/paymentcard/{paymentcardId:guid}")]
    [ProducesResponseType(typeof(PaymentCardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get paymentcard by id",
        Description = "Gets a paymentcard from id",
        OperationId = "GetPaymentCardById",
        Tags = new[] { "PaymentCard" })
    ]
    public override async Task<ActionResult<PaymentCardResponse>> HandleAsync([FromRoute] Guid paymentcardId, CancellationToken cancellationToken = default)
    {
        var paymentCardModel = await _getPaymentCardService.GetPaymentCardById(paymentcardId);

        if (paymentCardModel is null)
            return Problem(title: "PaymentCard could not be found",
                           detail: $"PaymentCard with id: '{paymentcardId}' not found",
                           statusCode: StatusCodes.Status404NotFound);

        var paymentCardResponse = PaymentCardResponseMapper.Map(paymentCardModel);

        return Ok(paymentCardResponse);
    }
}
