using PasswordManager.Password.ApplicationServices.Repositories.Operations;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Infrastructure.PasswordRepository;
using PasswordManager.Password.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace PasswordManager.Password.Infrastructure.OperationRepository;
public class OperationRepository : BaseRepository<Operation, OperationEntity>, IOperationRepository
{
    public OperationRepository(PasswordContext context) : base(context)
    {
    }

    public async Task<Operation?> GetByRequestId(string requestId)
    {
        var operationEntity = await GetOperationsDbSet()
            .Where(x => x.RequestId == requestId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        return operationEntity is null ? null : Map(operationEntity);
    }

    public async Task<ICollection<Operation>> GetPasswordOperations(Guid passwordId)
    {
        var password = await GetOperationsDbSet()
                    .Where(x => x.PasswordId == passwordId)
                    .AsNoTracking()
                    .ToListAsync();
        return password.Select(Map).ToImmutableHashSet();
    }

    private DbSet<OperationEntity> GetOperationsDbSet()
    {
        if (Context.PasswordOperations is null)
            throw new InvalidOperationException("Database configuration not setup correctly");
        return Context.PasswordOperations;
    }

    protected override Operation Map(OperationEntity entity)
    {
        return OperationMapper.Map(entity);
    }

    protected override OperationEntity Map(Operation model)
    {
        return OperationMapper.Map(model);
    }
}
