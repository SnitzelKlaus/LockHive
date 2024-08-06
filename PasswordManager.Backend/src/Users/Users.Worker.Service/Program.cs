using PasswordManager.Users.Infrastructure.Installers;
using PasswordManager.Users.Infrastructure.Extensions;
using PasswordManager.Users.Infrastructure.Startup;
using Microsoft.AspNetCore;

namespace PasswordManager.Users.Worker.Service;
public class Program
{
    public static async Task Main(string[] args)
    {
         var host = CreateWebHostBuilder(args).Build();

            var runOnStartupExecution = host.Services.GetRequiredService<IRunOnStartupExecution>();

            await runOnStartupExecution.RunAll();
            await host.RunAsync();
    }

    /// <summary>
    /// Creates an instance of the web host builder.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The web host builder.</returns>
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration(builder =>
               {
                   builder
                       .AddEnvironmentVariables()
                       .Build();
               })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   logging.AddConsole();
               })
               .ConfigureServices((context, collection) =>
               {
                   var configuration = context.Configuration;

                   var parameters = new DependencyInstallerOptions(configuration, context.HostingEnvironment);
                   collection.AddFromAssembly<Program>(parameters);
                   collection.AddFromAssembly<ServiceInstaller>(parameters);
               })
               .UseStartup<Startup>();
}
