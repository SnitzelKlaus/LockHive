namespace PasswordManager.PaymentCards.Infrastructure.BaseRepository;
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
    public bool Deleted { get; set; } = false;

    public BaseEntity(Guid id)
    {
        Id = id;
        CreatedUtc = DateTime.UtcNow;
        ModifiedUtc = DateTime.UtcNow;
    }

    public BaseEntity(Guid id, DateTime createdUtc, DateTime modifiedUtc, bool deleted)
    {
        Id = id;
        CreatedUtc = createdUtc;
        ModifiedUtc = modifiedUtc;
        Deleted = deleted;
    }
}
