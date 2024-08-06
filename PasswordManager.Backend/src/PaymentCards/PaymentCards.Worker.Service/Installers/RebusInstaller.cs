using Rebus.Bus;
using Rebus.Retry.Simple;
using Rebus.Config;
using PasswordManager.PaymentCards.Infrastructure.Installers;
using Rebus.Routing.TypeBased;
using PaymentCards.Messages.CreatePaymentCard;

namespace PasswordManager.PaymentCards.Worker.Service.Installers
{
    public class RebusInstaller : IDependencyInstaller
    {
        /// <summary>
        /// Events that this worker subscribes to
        /// </summary>
        private static readonly Type[] EventSubscriptionTypes = {
            // typeof(MyFeatureHasHappenedEvents)
        };

        public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
        {
            // Register handlers
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
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<CreatePaymentCardCommand>(Constants.ServiceBus.InputQueue))
                .Options(o =>
                {
                    o.RetryStrategy(maxDeliveryAttempts: 5);
                }), 
                onCreated: SubscribeToEvents);
        }

        /// <summary>
        /// Subscribe to Rebus Topics
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