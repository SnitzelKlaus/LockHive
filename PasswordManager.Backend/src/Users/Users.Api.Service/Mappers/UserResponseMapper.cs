using PasswordManager.Users.Api.Service.Models;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.Api.Service.Mappers;

/// <summary>
/// Handles the mapping of user models to user response objects.
/// </summary>
internal static class UserResponseMapper
{
    /// <summary>
    /// Maps a single UserModel instance to a UserResponse object.
    /// </summary>
    /// <param name="userModel">The user model to be mapped.</param>
    /// <returns>A UserResponse object populated with data from the provided UserModel.</returns>
    internal static UserResponse Map(UserModel userModel)
    {
        var passwordResponse = new UserResponse(userModel.Id, userModel.FirebaseId);

        return passwordResponse;
    }

    /// <summary>
    /// Maps a collection of UserModel instances to a collection of UserResponse objects.
    /// </summary>
    /// <param name="userModels">The collection of user models to be mapped.</param>
    /// <returns>A collection of UserResponse objects, each corresponding to one of the provided UserModel instances.</returns>
    internal static IEnumerable<UserResponse> Map(IEnumerable<UserModel> userModels)
    {
        return userModels.Select(Map);
    }
}
