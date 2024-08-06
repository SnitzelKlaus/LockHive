using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.Api.Service.Mappers;
/// <summary>
/// Provides methods for mapping user password models to user password responses.
/// </summary>
internal static class UserPasswordResponseMapper
{
    /// <summary>
    /// Maps a user password model to a user password response.
    /// </summary>
    /// <param name="model">The user password model to map.</param>
    /// <returns>A user password response mapped from the provided model.</returns>
    internal static UserPasswordResponse Map(UserPasswordModel model)
    {
        var userPasswordResponse = new UserPasswordResponse(model.UserId, model.PasswordId, model.Url, model.FriendlyName, model.Username, model.Password);

        return userPasswordResponse;
    }
}
