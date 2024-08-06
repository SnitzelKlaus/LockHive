using PasswordManager.PaymentCards.Api.Service.Models;
using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.Api.Service.Mappers;

internal static class PaymentCardResponseMapper
{
    internal static PaymentCardResponse Map(PaymentCardModel model)
    {
        var response = new PaymentCardResponse
        (
            model.Id,
            model.CardNumber,
            model.CardHolderName,
            model.ExpiryMonth,
            model.ExpiryYear,
            model.Cvv
        );

        return response;
    }

    internal static IEnumerable<PaymentCardResponse> Map(IEnumerable<PaymentCardModel>? models)
    {
        if (models is null) return Enumerable.Empty<PaymentCardResponse>();
        return models.Select(Map);
    }
}
