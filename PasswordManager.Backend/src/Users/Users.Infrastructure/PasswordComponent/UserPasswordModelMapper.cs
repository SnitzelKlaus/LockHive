
using PasswordManager.Users.Domain.User;
using Umbraco.Cloud.Passwordmanager.Password.Api.Client;

namespace PasswordManager.Users.Infrastructure.PasswordComponent;
/// <summary>
/// Maps between password response and user password models.
/// </summary>
internal static class UserPasswordModelMapper
{
    /// <summary>
    /// Converts PasswordResponse to UserPasswordModel.
    /// </summary>
    /// <param name="model">Source PasswordResponse.</param>
    /// <returns>Mapped UserPasswordModel.</returns>
    internal static UserPasswordModel Map(PasswordResponse model)
    {
        var userPasswordModel = new UserPasswordModel(model.UserId, model.Id, model.Url, model.FriendlyName, model.Username, model.Password);
        return userPasswordModel;
    }
}
