using Microsoft.Extensions.DependencyInjection;
using PasswordManager.PaymentCards.Infrastructure.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.PaymentCards.ApplicationServices.Repositories.Operations;
using PasswordManager.PaymentCards.ApplicationServices.Repositories.PaymentCard;
using PasswordManager.PaymentCards.Infrastructure.PaymentCardRepository;

namespace PasswordManager.PaymentCards.Infrastructure.Installers;

public sealed class ServiceInstaller : IDependencyInstaller
{
    public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
    {
        serviceCollection.AddTransient<IRunOnStartupExecution, RunOnStartupExecution>();
        AddRepositories(serviceCollection, options.Configuration);
    }

    private static void AddRepositories(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration[Constants.ConfigurationKeys.SqlDbConnectionString];

        serviceCollection.AddDbContext<PaymentCardContext>(options => options.UseSqlServer(connectionString));

        serviceCollection.AddScoped<IPaymentCardRepository, PaymentCardRepository.PaymentCardRepository>();
        serviceCollection.AddScoped<IOperationRepository, OperationRepository.OperationRepository>();
    }
}

