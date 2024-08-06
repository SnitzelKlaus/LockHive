using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.User.GetUser;
using PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
using PasswordManager.Users.ApplicationServices.UserPassword.DeleteUserPassword;
using PasswordManager.Users.ApplicationServices.UserPassword.GetUserPasswords;
using PasswordManager.Users.ApplicationServices.UserPassword.UpdateUserPassword;

namespace PasswordManager.Users.ApplicationServices.Extensions;
/// <summary>
/// Extension methods for IServiceCollection to register application services.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers application services in the dependency injection container.
    /// </summary>
    /// <param name="services">The collection of services to add to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddApplicationServiceServices(this IServiceCollection services)
    {
        //Add application service services
        //Use scoped as method to add services

        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<IGetUserService, GetUserService>();
        services.AddScoped<IGetUserPasswordsService, GetUserPasswordsService>();
        services.AddScoped<ICreateUserPasswordService, CreateUserPasswordService>();
        services.AddScoped<IDeleteUserPasswordService, DeleteUserPasswordService>();
        services.AddScoped<IUpdateUserPasswordService, UpdateUserPasswordService>();
        
        return services;
    }
}
