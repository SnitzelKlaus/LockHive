using PasswordManager.Users.Infrastructure.Installers;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Users.Messages.CreateUserPassword;

namespace PasswordManager.Users.Api.Service.Installers;
/// <summary>
/// Installer for configuring Rebus messaging for the API.
/// </summary>
public class RebusInstaller : IDependencyInstaller
{
    /// <summary>
    /// Installs Rebus messaging services for communication between components.
    /// </summary>
    /// <param name="serviceCollection">The service collection to which Rebus services will be added.</param>
    /// <param name="options">Options for dependency installation.</param>
    public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
    {
        // For integration tests, skip further configuration
        if (options.HostEnvironment.IsEnvironment("integration-test")) return;

        // Retrieve the service bus connection string
        var serviceBusConnectionString =
            options.Configuration[Infrastructure.Constants.ConfigurationKeys.ServiceBusConnectionString];

        // Ensure the service bus connection string is not empty
        if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
            throw new InvalidOperationException("Unable to resolve service bus connection string named " +
                                                $"{PasswordManager.Users.Infrastructure.Constants.ConfigurationKeys.ServiceBusConnectionString} " +
                                                "from environment variables");

        // Configure Rebus with Azure Service Bus transport
        serviceCollection.AddRebus((configure, provider) => configure
            .Logging(l => l.MicrosoftExtensionsLogging(provider.GetRequiredService<ILoggerFactory>()))
            .Transport(t => t.UseAzureServiceBusAsOneWayClient(serviceBusConnectionString))
            .Routing(r => r.TypeBased()
                .MapAssemblyOf<CreateUserPasswordCommand>(Infrastructure.Constants.ServiceBus.InputQueue))
        ); ;
    }
}