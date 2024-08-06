using Microsoft.AspNetCore;
using PasswordManager.PaymentCards.Infrastructure.Startup;
using PasswordManager.PaymentCards.Infrastructure.Installers;
using PasswordManager.PaymentCards.Infrastructure.Extensions;

namespace PasswordManager.PaymentCards.Api.Service;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateWebHostBuilder(args).Build();

        var runOnStartupExecution = host.Services.GetRequiredService<IRunOnStartupExecution>();

        await runOnStartupExecution.RunAll();
        await host.RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConsole();

                //You add logging to application insight here.
            })
            .ConfigureServices((hostingContext, collection) =>
            {
                var configuration = hostingContext.Configuration;

                var parameters = new DependencyInstallerOptions(configuration, hostingContext.HostingEnvironment);
                collection.AddFromAssembly<Program>(parameters);
                collection.AddFromAssembly<ServiceInstaller>(parameters);
            })
        .UseStartup<Startup>();
}
