using PasswordManager.Password.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PasswordManager.Password.Infrastructure.PasswordRepository;
public class PasswordConfiguration : BaseEntityConfiguration<PasswordEntity>
{
    public override void Configure(EntityTypeBuilder<PasswordEntity> builder)
    {
        base.Configure(builder);

        // UserId is required and should not be updated after it's been set
        builder.Property(p => p.UserId)
            .IsRequired()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(p => p.Url).IsRequired();
        builder.Property(p => p.FriendlyName).IsRequired();
        builder.Property(p => p.Password).IsRequired();
        builder.Property(p => p.Username).IsRequired();
        builder.Property(p => p.Deleted).IsRequired();
    }
}