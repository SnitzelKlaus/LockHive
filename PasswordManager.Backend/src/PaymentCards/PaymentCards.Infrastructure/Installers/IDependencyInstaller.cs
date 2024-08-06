using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager.PaymentCards.Infrastructure.Installers;
public interface IDependencyInstaller
{
    void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options);
}