using PasswordManager.PaymentCards.ApplicationServices.Repositories.Operations;
using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;
using PasswordManager.PaymentCards.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System;

namespace PasswordManager.PaymentCards.Infrastructure.OperationRepository;
public class OperationRepository : BaseRepository<Operation, OperationEntity, PaymentCardContext>, IOperationRepository
{
    public OperationRepository(PaymentCardContext context) : base(context, OperationMapper.Map, OperationMapper.Map)
    {
    }

    public async Task<Operation?> GetByRequestId(string requestId)
    {
        var operationEntity = await Context.PaymentCardsOperations
            .Where(x => x.RequestId == requestId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        return operationEntity is null ? null : MapEntityToModel(operationEntity);
    }

    public async Task<ICollection<Operation>> GetPaymentCardOperations(Guid securitykeyId)
    {
        var securitykey = await Context.PaymentCardsOperations
                    .Where(x => x.PaymentCardId == securitykeyId)
                    .AsNoTracking()
                    .ToListAsync();
        return securitykey.Select(MapEntityToModel).ToImmutableHashSet();
    }
}
