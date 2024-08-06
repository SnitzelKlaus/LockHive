using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager.Users.Infrastructure.Installers;
public interface IDependencyInstaller
{
    void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options);
}