using PasswordManager.Users.Infrastructure.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.Users.ApplicationServices.Repositories.Operations;
using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.Infrastructure.UserRepository;
using PasswordManager.Users.ApplicationServices.Components;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cloud.Passwordmanager.Password.Api.Client;
using Umbraco.Cloud.Passwordmanager.Keyvaults.Api.Client;

namespace PasswordManager.Users.Infrastructure.Installers;
/// <summary>
/// Configures and registers dependencies for services, repositories, and components within the application's service collection.
/// </summary>
public sealed class ServiceInstaller : IDependencyInstaller
{
    /// <summary>
    /// Installs dependencies into the provided service collection based on the given configuration.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add services to.</param>
    /// <param name="options">Options containing configuration for dependency installation.</param>
    public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
    {
        serviceCollection.AddTransient<IRunOnStartupExecution, RunOnStartupExecution>();
        AddRepositories(serviceCollection, options.Configuration);
        AddComponents(serviceCollection, options.Configuration);
    }

    /// <summary>
    /// Adds repository services to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add repositories to.</param>
    /// <param name="configuration">The application configuration.</param>
    private static void AddRepositories(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration[Constants.ConfigurationKeys.SqlDbConnectionString];

        serviceCollection.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

        serviceCollection.AddScoped<IUserRepository, UserRepository.UserRepository>();
        serviceCollection.AddScoped<IOperationRepository, OperationRepository.OperationRepository>();
    }

    /// <summary>
    /// Adds component services to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add components to.</param>
    /// <param name="configuration">The application configuration.</param>
    private static void AddComponents(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        SetupPasswordIntegration(serviceCollection, configuration);
        serviceCollection.AddScoped<IPasswordComponent, PasswordComponent.PasswordComponent>();

        SetupKeyVaultIntegration(serviceCollection, configuration);
        serviceCollection.AddScoped<IKeyVaultComponent, KeyVaultComponent.KeyVaultComponent>();
    }

    /// <summary>
    /// Configures HttpClient and service for password management integration.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    private static void SetupPasswordIntegration(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var httpClientName = Constants.HttpClientNames.Password;

        serviceCollection.AddHttpClient(httpClientName, c => { c.BaseAddress = new Uri("http://localhost:60440"); });

        serviceCollection.AddTransient<IPasswordmanagerPasswordApiClient, PasswordmanagerPasswordApiClient>(c =>
        {
            var clientFactory = c.GetRequiredService<IHttpClientFactory>();
            var client = clientFactory.CreateClient(httpClientName);

            return new PasswordmanagerPasswordApiClient(string.Empty, client);
        });
    }

    /// <summary>
    /// Configures HttpClient and service for KeyVault integration.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    private static void SetupKeyVaultIntegration(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var httpClientName = Constants.HttpClientNames.KeyVault;

        serviceCollection.AddHttpClient(httpClientName, c => { c.BaseAddress = new Uri("http://localhost:60431"); });

        serviceCollection.AddTransient<IPasswordmanagerKeyvaultsApiClient, PasswordmanagerKeyvaultsApiClient>(c =>
        {
            var clientFactory = c.GetRequiredService<IHttpClientFactory>();
            var client = clientFactory.CreateClient(httpClientName);

            return new PasswordmanagerKeyvaultsApiClient(string.Empty, client);
        });
    }
}

