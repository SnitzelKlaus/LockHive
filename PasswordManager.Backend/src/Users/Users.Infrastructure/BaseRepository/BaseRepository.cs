using PasswordManager.Users.ApplicationServices.Repositories;
using PasswordManager.Users.Domain;
using PasswordManager.Users.Infrastructure.UserRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace PasswordManager.Users.Infrastructure.BaseRepository;
/// <summary>
/// An abstract base repository implementing common CRUD operations for entities.
/// </summary>
/// <typeparam name="T">The type of the model in the domain layer.</typeparam>
/// <typeparam name="TE">The type of the entity in the database layer.</typeparam>
public abstract class BaseRepository<T, TE> : IBaseRepository<T> where T : BaseModel where TE : BaseEntity
{
    private readonly DbSet<TE> _dbSet;
    protected readonly UserContext Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T, TE}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    protected BaseRepository(UserContext context)
    {
        Context = context;
        _dbSet = context.Set<TE>();
    }

    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The corresponding model or null if not found.</returns>
    public async Task<T?> Get(Guid id)
    {
        var fetchedEntity = await _dbSet
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.Id == id);
        return fetchedEntity is null ? null : Map(fetchedEntity);
    }

    /// <summary>
    /// Retrieves all entities of type TE.
    /// </summary>
    /// <returns>A collection of all models of type T.</returns>
    public async Task<ICollection<T>> GetAll()
    {
        var all = await _dbSet.ToListAsync();
        return all.Select(Map).ToImmutableHashSet();
    }

    /// <summary>
    /// Adds a new entity or updates an existing one.
    /// </summary>
    /// <param name="baseModel">The model to add or update.</param>
    /// <returns>The added or updated model.</returns>
    public async Task<T> Upsert(T baseModel)
    {
        var existingEntity = await GetTracked(baseModel.Id);

        if (existingEntity == null) return await Add(baseModel);
        // right now existing is tracked by ef core - use this to apply updates
        var updatedEntity = Map(baseModel);

        Context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity); // all simple values on entity
        existingEntity.ModifiedUtc = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        Context.ChangeTracker.Clear();
        return Map(existingEntity);
    }

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="baseModel">The model to add.</param>
    /// <returns>The added model.</returns>
    private async Task<T> Add(T baseModel)
    {
        var entity = Map(baseModel);
        await _dbSet.AddAsync(entity);
        await SaveAsync(entity);

        Context.ChangeTracker.Clear();

        return Map(entity);
    }

    /// <summary>
    /// Retrieves an entity by its identifier, while keeping it tracked by the DbContext.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The tracked entity, if found; otherwise, null.</returns>
    private async Task<TE?> GetTracked(Guid id)
    {
        var fetchedEntity = await _dbSet
            .SingleOrDefaultAsync(t => t.Id == id);
        return fetchedEntity;
    }

    /// <summary>
    /// Maps an entity to its model.
    /// </summary>
    /// <param name="entity">The entity to map.</param>
    /// <returns>The mapped model.</returns>
    protected abstract T Map(TE entity);

    /// <summary>
    /// Maps a model to its entity.
    /// </summary>
    /// <param name="model">The model to map.</param>
    /// <returns>The mapped entity.</returns>
    protected abstract TE Map(T model);

    /// <summary>
    /// Saves changes to the database asynchronously.
    /// </summary>
    /// <param name="baseEntity">The entity to save.</param>
    private async Task SaveAsync(TE baseEntity)
    {
        baseEntity.ModifiedUtc = DateTime.UtcNow;

        await Context.SaveChangesAsync();
    }
}
