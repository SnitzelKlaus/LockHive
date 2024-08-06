using PasswordManager.Users.ApplicationServices.Repositories.Operations;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Infrastructure.UserRepository;
using PasswordManager.Users.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace PasswordManager.Users.Infrastructure.OperationRepository;
/// <summary>
/// Handles operation data storage and retrieval.
/// </summary>
public class OperationRepository : BaseRepository<Operation, OperationEntity>, IOperationRepository
{
    /// <summary>
    /// Initializes a new instance with a database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OperationRepository(UserContext context) : base(context)
    {
    }

    /// <summary>
    /// Finds an operation by its request ID.
    /// </summary>
    /// <param name="requestId">The request ID to search for.</param>
    /// <returns>The operation if found; otherwise, null.</returns>
    public async Task<Operation?> GetByRequestId(string requestId)
    {
        var operationEntity = await GetOperationsDbSet()
            .Where(x => x.RequestId == requestId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        return operationEntity is null ? null : Map(operationEntity);
    }

    /// <summary>
    /// Retrieves all operations for a specific user.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <returns>A collection of operations.</returns>
    public async Task<ICollection<Operation>> GetUserOperations(Guid userId)
    {
        var user = await GetOperationsDbSet()
                    .Where(x => x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
        return user.Select(Map).ToImmutableHashSet();
    }

    /// <summary>
    /// Gets the DbSet for operations.
    /// </summary>
    /// <returns>The operations DbSet.</returns>
    private DbSet<OperationEntity> GetOperationsDbSet()
    {
        if (Context.UsersOperations is null)
            throw new InvalidOperationException("Database configuration not setup correctly");
        return Context.UsersOperations;
    }

    /// <summary>
    /// Maps an OperationEntity to an Operation.
    /// </summary>
    /// <param name="entity">The entity to map.</param>
    /// <returns>The mapped operation.</returns>
    protected override Operation Map(OperationEntity entity)
    {
        return OperationMapper.Map(entity);
    }

    /// <summary>
    /// Maps an Operation to an OperationEntity.
    /// </summary>
    /// <param name="model">The model to map.</param>
    /// <returns>The mapped entity.</returns>
    protected override OperationEntity Map(Operation model)
    {
        return OperationMapper.Map(model);
    }
}
