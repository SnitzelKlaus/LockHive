using PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
using PasswordManager.PaymentCards.Domain.PaymentCards;
using PasswordManager.PaymentCards.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;
public class PaymentCardRepository : BaseRepository<PaymentCardModel, PaymentCardEntity, PaymentCardContext>, IPaymentCardRepository
{
    public PaymentCardRepository(PaymentCardContext context) : base(context, PaymentCardEntityMapper.Map, PaymentCardEntityMapper.Map)
    {
    }

    public async Task<IEnumerable<PaymentCardModel>?> GetByUserId(Guid userId) => await Context.PaymentCards
        .Where(x => x.UserId == userId && !x.Deleted)
        .AsNoTracking()
        .Select(x => MapEntityToModel(x))
        .ToListAsync();
}
