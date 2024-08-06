using PasswordManager.Users.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Users.Infrastructure.UserRepository;
/// <summary>
/// Configures the entity model for the <see cref="UserEntity"/> class, defining properties, relationships, and database mappings.
/// Inherits from <see cref="BaseEntityConfiguration{UserEntity}"/> to apply base entity configurations and extend them with configurations specific to the <see cref="UserEntity"/>.
/// </summary>
public class UserConfiguration : BaseEntityConfiguration<UserEntity>
{
    /// <summary>
    /// Configures the entity model of <see cref="UserEntity"/>.
    /// This method is called by Entity Framework Core to apply entity type configurations using the Fluent API.
    /// </summary>
    /// <param name="builder">Provides a simple API surface for configuring an <see cref="EntityTypeBuilder{UserEntity}"/> for the entity type <see cref="UserEntity"/>.</param>
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.FirebaseId).IsRequired();
        builder.Property(p => p.SecretKey).IsRequired();
    }
}