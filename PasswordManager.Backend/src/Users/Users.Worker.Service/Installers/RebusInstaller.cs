using Rebus.Bus;
using Rebus.Retry.Simple;
using Rebus.Config;
using PasswordManager.Users.Infrastructure.Installers;

namespace PasswordManager.Users.Worker.Service.Installers
{
    /// <summary>
    /// Installs Rebus dependencies and configures Rebus during application startup.
    /// </summary>
    public class RebusInstaller : IDependencyInstaller
    {
        /// <summary>
        /// Events that this worker subscribes to.
        /// </summary>
        private static readonly Type[] EventSubscriptionTypes = {
            // typeof(MyFeatureHasHappenedEvents)
        };

        /// <summary>
        /// Installs Rebus dependencies and configures Rebus.
        /// </summary>
        public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
        {
            // Register Rebus message handlers
            serviceCollection.AutoRegisterHandlersFromAssemblyOf<Program>();

            // Configure Rebus
            var serviceBusConnectionString =
                        options.Configuration[Infrastructure.Constants.ConfigurationKeys.ServiceBusConnectionString];

            if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
                throw new InvalidOperationException($"Unable to resolve service bus connection string named " +
                                                    $"{Infrastructure.Constants.ConfigurationKeys.ServiceBusConnectionString} " +
                                                    "from environment variables");

            serviceCollection.AddRebus((configure, provider) => configure
                .Logging(l => l.MicrosoftExtensionsLogging(provider.GetRequiredService<ILoggerFactory>()))
                .Transport(t =>
                {
                    t.UseAzureServiceBus(
                            connectionString: serviceBusConnectionString,
                            inputQueueAddress: Constants.ServiceBus.InputQueue)
                        .AutomaticallyRenewPeekLock();
                    t.UseNativeDeadlettering();
                })
                //Routing here. Map command
                //Example --> .MapAssemblyOf<CreateCustomerCommand>(Constants.ServiceBus.InputQueue))
                .Options(o =>
                {
                    o.RetryStrategy(maxDeliveryAttempts: 5);
                }), 
                onCreated: SubscribeToEvents);
        }

        /// <summary>
        /// Subscribe to Rebus topics.
        /// </summary>
        private static async Task SubscribeToEvents(IBus bus)
        {
            foreach (var subscriptionType in EventSubscriptionTypes)
            {
                bus.Advanced.SyncBus.Subscribe(subscriptionType);
            }
        }
    }
}