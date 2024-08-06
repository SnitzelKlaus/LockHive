using PasswordManager.PaymentCards.Infrastructure.BaseRepository;

namespace PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;

public class PaymentCardEntity : BaseEntity
{
    public Guid UserId { get; }
    public string CardNumber { get; private set; }
    public string CardHolderName { get; private set; }
    public int ExpiryMonth { get; private set; }
    public int ExpiryYear { get; private set; }
    public string Cvv { get; private set; }

    public PaymentCardEntity(Guid id, DateTime createdUtc, DateTime modifiedUtc, bool deleted, Guid userId, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv) 
        : base(id, createdUtc, modifiedUtc, deleted)
    {
        UserId = userId;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Cvv = cvv;
    }
}