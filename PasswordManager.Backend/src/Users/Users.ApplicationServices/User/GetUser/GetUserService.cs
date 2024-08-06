using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.ApplicationServices.User.GetUser;
/// <summary>
/// Service for retrieving user information.
/// </summary>
internal class GetUserService : IGetUserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository for accessing user data.</param>
    public GetUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieves user information based on the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user model corresponding to the specified user ID, or null if the user is not found.</returns>
    public async Task<UserModel?> GetUser(Guid userId)
    {
        var user = await _userRepository.Get(userId);

        return user;
    }
}
