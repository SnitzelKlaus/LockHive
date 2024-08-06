using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.TestHelpers;
using Microsoft.AspNetCore.Mvc.Testing;
using PasswordManager.Users.Api.Service;

namespace Users.Integration.Tests;
internal class BaseApiServiceWebApplicationFactory : WebApplicationFactory<Startup>
{
    private readonly FakeBus _fakeBus = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "integration-test.settings.json");
        File.WriteAllText(configPath,
            File.ReadAllText(configPath)
                .Replace("$$PORT_NUMBER$$", $"{IntegrationTestAssemblyFixtureSetup.MappedPort}"));
        builder.ConfigureAppConfiguration((_, conf) => { conf.AddJsonFile(configPath); });
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(_ => { });

        builder.ConfigureTestServices(services =>
        {
            services.AddHttpClient();
            services.AddScoped<IBus>(_ => _fakeBus);
        });

        builder.UseEnvironment("integration-test");
    }

    public FakeBus GetUsedBus()
    {
        return _fakeBus;
    }
}
