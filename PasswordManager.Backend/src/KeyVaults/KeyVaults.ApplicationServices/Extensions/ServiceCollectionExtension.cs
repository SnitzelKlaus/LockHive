using Microsoft.Extensions.DependencyInjection;
using PasswordManager.KeyVaults.ApplicationServices.Protection;

namespace PasswordManager.KeyVaults.ApplicationServices.Extensions;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServiceServices(this IServiceCollection services)
    {
        // Application services
        services.AddScoped<IProtectionService, ProtectionService>();

        return services;
    }
}
