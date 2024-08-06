using PasswordManager.PaymentCards.Domain;

namespace PasswordManager.PaymentCards.ApplicationServices.Repositories;
public interface IBaseRepository<TModel> where TModel : BaseModel
{
    Task<TModel?> GetById(Guid id);
    Task<ICollection<TModel>> GetAll();
    Task<TModel> Upsert(TModel model);
    Task Delete(Guid id);
}