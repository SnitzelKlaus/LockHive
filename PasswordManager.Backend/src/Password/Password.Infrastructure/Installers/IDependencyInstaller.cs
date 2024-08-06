using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager.Password.Infrastructure.Installers;
public interface IDependencyInstaller
{
    void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options);
}