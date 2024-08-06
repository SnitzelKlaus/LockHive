namespace PasswordManager.Password.Infrastructure.BaseRepository;
public class BaseEntity
{
    public Guid Id { get; }
    public DateTime CreatedUtc { get; }
    public DateTime ModifiedUtc { get; set; }
    public bool Deleted { get; set; } = false;

    public BaseEntity(Guid id, DateTime createdUtc, DateTime modifiedUtc)
    {
        Id = id;
        CreatedUtc = createdUtc;
        ModifiedUtc = modifiedUtc;
    }
    public BaseEntity(Guid id)
    {
        Id = id;
        CreatedUtc = DateTime.UtcNow;
        ModifiedUtc = DateTime.UtcNow;
    }
}
