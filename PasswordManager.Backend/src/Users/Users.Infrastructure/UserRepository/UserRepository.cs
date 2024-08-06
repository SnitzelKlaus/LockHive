using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.Domain.User;
using PasswordManager.Users.Infrastructure.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Users.Infrastructure.UserRepository;
/// <summary>
/// Represents a repository for managing user data, providing an abstraction over the data access layer.
/// This repository facilitates operations such as querying, updating, and persisting user-related data.
/// Inherits from <see cref="BaseRepository{TModel, TEntity}"/> to leverage common CRUD operations.
/// Implements <see cref="IUserRepository"/> to enforce a contract for user-specific data operations.
/// </summary>
public class UserRepository : BaseRepository<UserModel, UserEntity>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="context">The database context for user entities.</param>
    public UserRepository(UserContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves the <see cref="DbSet{TEntity}"/> for <see cref="UserEntity"/> from the current context.
    /// </summary>
    /// <returns>The <see cref="DbSet{TEntity}"/> for user entities.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user DbSet is not configured correctly in the context.</exception>
    private DbSet<UserEntity> GetUserDbSet()
    {
        if (Context.Users is null)
            throw new InvalidOperationException("Database configuration not setup correctly");
        return Context.Users;
    }

    /// <summary>
    /// Maps a <see cref="UserEntity"/> to a <see cref="UserModel"/>.
    /// </summary>
    /// <param name="entity">The user entity to map.</param>
    /// <returns>The mapped user model.</returns>
    protected override UserModel Map(UserEntity entity)
    {
        return UserEntityMapper.Map(entity);
    }

    /// <summary>
    /// Maps a <see cref="UserModel"/> to a <see cref="UserEntity"/>.
    /// </summary>
    /// <param name="model">The user model to map.</param>
    /// <returns>The mapped user entity.</returns>
    protected override UserEntity Map(UserModel model)
    {
        return UserEntityMapper.Map(model);
    }
}
