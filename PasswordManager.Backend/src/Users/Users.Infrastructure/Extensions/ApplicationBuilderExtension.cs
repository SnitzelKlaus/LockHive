using PasswordManager.Users.Infrastructure.UserRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager.Users.Infrastructure.Extensions;
/// <summary>
/// Provides extension methods for IApplicationBuilder to enhance application configuration and initialization.
/// </summary>
public static class ApplicationBuilderExtension
{
    /// <summary>
    /// Ensures that the database is migrated to the latest version at application startup.
    /// This method applies any pending migrations for the context to the database.
    /// It will create the database if it does not already exist.
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance.</param>
    public static void EnsureDatabaseMigrated(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<UserContext>();

        context.Database.Migrate();
    }
}