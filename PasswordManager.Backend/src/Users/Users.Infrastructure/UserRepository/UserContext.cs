using PasswordManager.Users.Infrastructure.OperationRepository;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Users.Infrastructure.UserRepository;
/// <summary>
/// Provides a context for user-related entities, enabling interaction with the database for operations related to users and their operations.
/// Inherits from <see cref="DbContext"/> to leverage the Entity Framework Core functionality for ORM (Object-Relational Mapping).
/// </summary>
public class UserContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserContext"/> class with the specified options.
    /// The options include configurations such as the database provider (e.g., SQL Server, SQLite) and connection string.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<OperationEntity>? UsersOperations { get; set; }

    public DbSet<UserEntity>? Users { get; set; }

    /// <summary>
    /// The configuration provided here will be applied to the model before it is used to initialize the context, allowing customization of the model (such as specifying table names, schemas, indexes, relationships, etc.).
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OperationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
