namespace PasswordManager.PaymentCards.Domain.PaymentCards;
public class PaymentCardModel : BaseModel
{
    public Guid UserId { get; }
    public string CardNumber { get; private set; }
    public string CardHolderName { get; private set; }
    public int ExpiryMonth { get; private set; }
    public int ExpiryYear { get; private set; }
    public string Cvv { get; private set; }

    public PaymentCardModel(Guid id, Guid userId, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv) : base(id)
    {
        UserId = userId;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Cvv = cvv;
    }

    public PaymentCardModel(Guid id, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv) : base(id)
    {
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Cvv = cvv;
    }

    public PaymentCardModel(Guid id, DateTime createdUtc, DateTime modifiedUtc, bool deleted, Guid userId, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv) : base(id, createdUtc, modifiedUtc, deleted)
    {
        UserId = userId;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Cvv = cvv;
    }

    public static PaymentCardModel CreatePaymentCard(Guid userId, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv)
    {
        return new PaymentCardModel(Guid.NewGuid(), userId, cardNumber, cardHolderName, expiryMonth, expiryYear, cvv);
    }

    public static PaymentCardModel UpdatePaymentCard(Guid id, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv)
    {
        return new PaymentCardModel(id, cardNumber, cardHolderName, expiryMonth, expiryYear, cvv);
    }
}
