using PasswordManager.PaymentCards.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;
public class PaymentCardConfiguration : BaseEntityConfiguration<PaymentCardEntity>
{
    public override void Configure(EntityTypeBuilder<PaymentCardEntity> builder)
    {
        base.Configure(builder);

        // UserId is required and should not be updated after it's been set
        builder.Property(x => x.UserId)
            .IsRequired()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CardNumber).IsRequired();
        builder.Property(x => x.CardHolderName).IsRequired();
        builder.Property(x => x.ExpiryMonth).IsRequired();
        builder.Property(x => x.ExpiryYear).IsRequired();
    }
}