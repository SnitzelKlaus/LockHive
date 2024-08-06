using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Users.Infrastructure.BaseRepository;
/// <summary>
/// Provides a base configuration for entity types in the database context.
/// </summary>
/// <typeparam name="T">The type of entity being configured.</typeparam>
public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    private const string ClusterIdName = "ClusterId";

    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The builder to use to configure the entity type.</param>
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id).IsClustered(false);
        builder.Property(e => e.CreatedUtc).IsRequired();

        // Setting the Clustering Key
        // As Mentioned in: https://stackoverflow.com/a/11938495
        builder.Property<int>(ClusterIdName)
            .ValueGeneratedOnAdd()
            .Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.HasIndex(ClusterIdName)
            .IsUnique()
            .IsClustered();
    }
}