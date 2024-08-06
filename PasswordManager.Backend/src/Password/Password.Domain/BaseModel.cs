namespace PasswordManager.Password.Domain;
public class BaseModel
{
    public Guid Id { get; private protected init; }
    public DateTime CreatedUtc { get; protected init; } 
    public DateTime ModifiedUtc { get; private protected init; }
    public bool Deleted { get; protected init; }

    protected BaseModel(Guid id)
    {
        Id = id;
        CreatedUtc = DateTime.UtcNow;
        ModifiedUtc = DateTime.UtcNow;
        Deleted = false;
    }

    protected BaseModel()
    {

    }

    protected BaseModel(Guid id, DateTime createdUtc, DateTime modifiedUtc, bool deleted)
    {
        Id = id;
        CreatedUtc = createdUtc;
        ModifiedUtc = modifiedUtc;
        Deleted = deleted;
    }
}
