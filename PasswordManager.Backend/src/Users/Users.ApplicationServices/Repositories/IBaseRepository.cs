using PasswordManager.Users.Domain;

namespace PasswordManager.Users.ApplicationServices.Repositories;
/// <summary>
/// Interface for the base repository handling CRUD operations for entities of type T.
/// </summary>
/// <typeparam name="T">The type of entity handled by the repository.</typeparam>
/// 
public interface IBaseRepository<T> where T : BaseModel
{
    /// <summary>
    /// Retrieves the entity with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity corresponding to the specified ID, or null if not found.</returns>
    Task<T?> Get(Guid id);

    /// <summary>
    /// Retrieves all entities of type T.
    /// </summary>
    /// <returns>A collection of all entities of type T.</returns>
    Task<ICollection<T>> GetAll();

    /// <summary>
    /// Inserts or updates the specified entity.
    /// </summary>
    /// <param name="baseModel">The entity to insert or update.</param>
    /// <returns>The inserted or updated entity.</returns>
    Task<T> Upsert(T baseModel);
}
