using PasswordManager.Password.Infrastructure.OperationRepository;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Password.Infrastructure.PasswordRepository;
public class PasswordContext : DbContext
{
    public PasswordContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OperationEntity>? PasswordOperations { get; set; }
    public DbSet<PasswordEntity>? Passwords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OperationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordConfiguration());
    }
}
