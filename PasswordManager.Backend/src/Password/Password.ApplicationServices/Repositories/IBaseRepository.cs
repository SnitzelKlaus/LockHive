using PasswordManager.Password.Domain;

namespace PasswordManager.Password.ApplicationServices.Repositories;
public interface IBaseRepository<T> where T : BaseModel
{
    Task<T?> Get(Guid id);
    Task<ICollection<T>> GetAll();
    Task<T> Upsert(T baseModel);
    Task<T> Add(T baseModel);
    Task Delete(Guid id);
}
