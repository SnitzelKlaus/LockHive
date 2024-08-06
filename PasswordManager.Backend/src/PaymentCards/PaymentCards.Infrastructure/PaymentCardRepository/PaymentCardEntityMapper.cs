using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;
internal static class PaymentCardEntityMapper
{
    internal static PaymentCardEntity Map(PaymentCardModel model)
    {
        return new PaymentCardEntity(
            model.Id,
            model.CreatedUtc,
            model.ModifiedUtc,
            model.Deleted,
            model.UserId,
            model.CardNumber,
            model.CardHolderName,
            model.ExpiryMonth,
            model.ExpiryYear,
            model.Cvv
            );
    }

    internal static PaymentCardModel Map(PaymentCardEntity entity)
    {
        return new PaymentCardModel(
            entity.Id,
            entity.CreatedUtc,
            entity.ModifiedUtc,
            entity.Deleted,
            entity.UserId,
            entity.CardNumber,
            entity.CardHolderName,
            entity.ExpiryMonth,
            entity.ExpiryYear,
            entity.Cvv
            );
    }
}