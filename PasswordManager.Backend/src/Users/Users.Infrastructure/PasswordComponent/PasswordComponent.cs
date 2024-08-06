using Microsoft.Extensions.Logging;
using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.Domain.User;
using Umbraco.Cloud.Passwordmanager.Password.Api.Client;

namespace PasswordManager.Users.Infrastructure.PasswordComponent;
/// <summary>
/// Implements password management operations, integrating with an external password management API.
/// </summary>
public sealed class PasswordComponent : IPasswordComponent
{
    private readonly IPasswordmanagerPasswordApiClient _passwordmanagerPasswordApiClient;
    private readonly ILogger<PasswordComponent> _logger;

    /// <summary>
    /// Initializes a new instance with required services.
    /// </summary>
    /// <param name="passwordmanagerPasswordApiClient">API client for password operations.</param>
    /// <param name="logger">Logger for logging operations.</param>
    public PasswordComponent(IPasswordmanagerPasswordApiClient passwordmanagerPasswordApiClient, ILogger<PasswordComponent> logger)
    {
        _passwordmanagerPasswordApiClient = passwordmanagerPasswordApiClient;
        _logger = logger;
    }

    /// <summary>
    /// Creates a password for a user.
    /// </summary>
    /// <param name="userPasswordModel">The password model to create.</param>
    public async Task CreateUserPassword(UserPasswordModel userPasswordModel)
    {
        try
        {
            _logger.LogInformation("Requesting to create password");
            await _passwordmanagerPasswordApiClient.CreatePasswordAsync(new CreatePasswordRequestWithBody(
                userPasswordModel.UserId.ToString(),
                new CreatePasswordRequestDetails(
                    userPasswordModel.FriendlyName, 
                    userPasswordModel.Password, 
                    userPasswordModel.Url, 
                    userPasswordModel.UserId, 
                    userPasswordModel.Username
                    )));
        }
        catch (ApiException exception)
        {
            _logger.LogError(exception, "Could not request password, service returned {StatusCode} and message {ErrorMessage}",
               exception.StatusCode, exception.Message);

            throw new PasswordComponentException("Error calling PasswordApiClient.CreatePasswordAsync", exception);
        }
    }

    /// <summary>
    /// Retrieves all passwords for a specific user.
    /// </summary>
    /// <param name="userId">User's unique identifier.</param>
    /// <returns>A collection of user password models.</returns>
    public async Task<IEnumerable<UserPasswordModel>> GetUserPasswords(Guid userId)
    {
        try
        {
            var passwordsResponse = await _passwordmanagerPasswordApiClient.GetPasswordsFromUserIdAsync(userId);

            var passwordsResponseResult = passwordsResponse.Result;

            return passwordsResponseResult.PasswordsResponses.Select(UserPasswordModelMapper.Map);
        }catch(ApiException exception)
        {
            throw new PasswordComponentException("Error calling PasswordApiClient.GetPasswordAsync", exception);
        }
    }

    /// <summary>
    /// Gets user passwords by URL.
    /// </summary>
    /// <param name="userId">User's unique identifier.</param>
    /// <param name="url">URL associated with passwords.</param>
    /// <returns>A collection of user password models.</returns>
    public async  Task<IEnumerable<UserPasswordModel>> GetUserPasswordsFromUrl(Guid userId, string url)
    {
        try
        {
            var passwordsResponse = await _passwordmanagerPasswordApiClient.GetPasswordsByUserIdAndUrlAsync(userId, url);

            var passwordsResponseResult = passwordsResponse.Result;

            return passwordsResponseResult.PasswordsResponses.Select(UserPasswordModelMapper.Map);
        }
        catch (ApiException exception)
        {
            throw new PasswordComponentException("Error calling PasswordApiClient.GetPasswordAsync", exception);
        }
    }

    /// <summary>
    /// Updates a specified user's password.
    /// </summary>
    /// <param name="userPasswordModel">Model with updated password info.</param>
    public async Task UpdateUserPassword(UserPasswordModel userPasswordModel)
    {
        try
        {
            var updatePasswordRequest = await _passwordmanagerPasswordApiClient.UpdatePasswordAsync(
                userPasswordModel.PasswordId, 
                userPasswordModel.UserId.ToString(), 
                new UpdatePasswordRequestDetails(
                    userPasswordModel.FriendlyName, 
                    userPasswordModel.Password, 
                    userPasswordModel.Url,
                    userPasswordModel.Username
                ));
        }
        catch(ApiException exception)
        {
            throw new PasswordComponentException("Error calling PasswordApiClient.UpdatePasswordAsync", exception);
        }
    }

    /// <summary>
    /// Deletes a user's password.
    /// </summary>
    /// <param name="passwordId">The password's unique identifier.</param>
    /// <param name="createdByUserId">The user's unique identifier.</param>
    public async Task DeleteUserPassword(Guid passwordId, string createdByUserId)
    {
        try
        {
            var deletePasswordRequest = await _passwordmanagerPasswordApiClient.DeletePasswordAsync(passwordId, createdByUserId);
        }
        catch (ApiException exception)
        {
            throw new PasswordComponentException("Error calling PasswordApiClient.UpdatePasswordAsync", exception);
        }
    }

    /// <summary>
    /// Generates a random password of a given length.
    /// </summary>
    /// <param name="length">The length of the password to generate.</param>
    /// <returns>The generated password as a string.</returns>
    public async Task<string> GenerateUserPassword(int length)
    {
        try
        {
            var generatePasswordRequest = await _passwordmanagerPasswordApiClient.GeneratePasswordAsync(length);
            return generatePasswordRequest.Result.Password;
        }
        catch (ApiException exception)
        {
            throw new PasswordComponentException("Error calling PasswordApiClient.UpdatePasswordAsync", exception);
        }
    }
}
