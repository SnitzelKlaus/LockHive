using PasswordManager.PaymentCards.Infrastructure.Installers;
using PasswordManager.PaymentCards.ApplicationServices.Extensions;

namespace PasswordManager.PaymentCards.Worker.Service.Installers
{
    public class ServiceInstaller : IDependencyInstaller
    {
        public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
        {
            serviceCollection.AddApplicationServiceServices();
        }
    }
}