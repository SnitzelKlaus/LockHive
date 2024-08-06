using PasswordManager.PaymentCards.Infrastructure.OperationRepository;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;
public class PaymentCardContext : DbContext
{
    public PaymentCardContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OperationEntity> PaymentCardsOperations { get; set; }
    public DbSet<PaymentCardEntity> PaymentCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OperationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentCardConfiguration());
    }
}
