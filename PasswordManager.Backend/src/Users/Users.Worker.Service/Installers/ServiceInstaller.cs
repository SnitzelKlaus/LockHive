using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
using PasswordManager.Users.ApplicationServices.UserPassword.DeleteUserPassword;
using PasswordManager.Users.ApplicationServices.UserPassword.UpdateUserPassword;
using PasswordManager.Users.Infrastructure.Installers;

namespace PasswordManager.Users.Worker.Service.Installers
{
    /// <summary>
    /// Installs service dependencies during application startup.
    /// </summary>
    public class ServiceInstaller : IDependencyInstaller
    {
        /// <summary>
        /// Installs service dependencies.
        /// </summary>
        /// <param name="serviceCollection">The collection of services to install.</param>
        /// <param name="options">The options for dependency installation.</param>
        public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
        {
            serviceCollection.AddScoped<IOperationService, OperationService>();
            serviceCollection.AddScoped<ICreateUserPasswordService, CreateUserPasswordService>();
            serviceCollection.AddScoped<IDeleteUserPasswordService, DeleteUserPasswordService>();
            serviceCollection.AddScoped<IUpdateUserPasswordService, UpdateUserPasswordService>();
        }
    }
}