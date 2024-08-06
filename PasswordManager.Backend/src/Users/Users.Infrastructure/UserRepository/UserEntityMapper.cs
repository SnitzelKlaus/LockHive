using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.Infrastructure.UserRepository;
/// <summary>
/// Provides utility methods for mapping between user models and entities.
/// This class facilitates the conversion of data between the domain layer represented by models and the data access layer represented by entities.
/// </summary>
internal static class UserEntityMapper
{
    /// <summary>
    /// Maps a <see cref="UserModel"/> to a <see cref="UserEntity"/>.
    /// </summary>
    /// <param name="model">The user model to map.</param>
    /// <returns>A new <see cref="UserEntity"/> instance populated with data from the provided model.</returns>
    internal static UserEntity Map(UserModel model)
    {
        return new UserEntity(
            model.Id,
            model.CreatedUtc,
            model.ModifiedUtc,
            model.FirebaseId,
            model.SecretKey
            );
    }

    /// <summary>
    /// Maps a <see cref="UserEntity"/> to a <see cref="UserModel"/>.
    /// </summary>
    /// <param name="entity">The user entity to map.</param>
    /// <returns>A new <see cref="UserModel"/> instance populated with data from the provided entity.</returns>
    internal static UserModel Map(UserEntity entity)
    {
        return new UserModel(
            entity.Id,
            entity.CreatedUtc,
            entity.ModifiedUtc,
            entity.Deleted,
            entity.FirebaseId,
            entity.SecretKey
            );
    }
}
