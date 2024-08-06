using PasswordManager.PaymentCards.ApplicationServices.Repositories;
using PasswordManager.PaymentCards.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace PasswordManager.PaymentCards.Infrastructure.BaseRepository;
public abstract class BaseRepository<TModel, TEntity, TContext> : IBaseRepository<TModel>
    where TModel : BaseModel
    where TEntity : BaseEntity
    where TContext : DbContext
{
    protected readonly TContext Context;
    private readonly DbSet<TEntity> _dbSet;

    // Delegates for mapping
    protected Func<TEntity, TModel> MapEntityToModel { get; set; }
    protected Func<TModel, TEntity> MapModelToEntity { get; set; }

    protected BaseRepository(TContext context, Func<TEntity, TModel> mapEntityToModel, Func<TModel, TEntity> mapModelToEntity)
    {
        Context = context;
        _dbSet = context.Set<TEntity>();
        MapEntityToModel = mapEntityToModel ?? throw new ArgumentNullException(nameof(mapEntityToModel));
        MapModelToEntity = mapModelToEntity ?? throw new ArgumentNullException(nameof(mapModelToEntity));
    }

    public async Task<TModel?> GetById(Guid id) =>
        await _dbSet
            .AsNoTracking()
            .Where(e => e.Id == id && !e.Deleted)
            .Select(e => MapEntityToModel(e))
            .SingleOrDefaultAsync();

    public async Task<ICollection<TModel>> GetAll()
    {
        var models = await _dbSet
            .AsNoTracking()
            .Where(e => !e.Deleted)
            .Select(e => MapEntityToModel(e))
            .ToListAsync();

        return models.ToImmutableHashSet();
    }

    public async Task<TModel> Upsert(TModel model)
    {
        var entity = MapModelToEntity(model);
        var existingEntity = await _dbSet.FindAsync(entity.Id);

        // If the entity exists, update it. Otherwise, add it.
        if (existingEntity != null)
        {
            Context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await SaveAsync(existingEntity);
        }
        else
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync(entity);
        }

        // Returns the model
        return MapEntityToModel(existingEntity ?? entity);
    }

    public async Task Delete(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return;

        entity.Deleted = true;
        await SaveAsync(entity);
    }

    private async Task SaveAsync(TEntity baseEntity)
    {
        baseEntity.ModifiedUtc = DateTime.UtcNow;
        await Context.SaveChangesAsync();

        Context.ChangeTracker.Clear();
    }
}