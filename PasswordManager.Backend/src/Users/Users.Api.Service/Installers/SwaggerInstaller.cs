using PasswordManager.Users.Infrastructure.Installers;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PasswordManager.Users.Api.Service.Installers;
/// <summary>
/// Installer for configuring Swagger documentation generation for the API.
/// </summary>
public class SwaggerInstaller : IDependencyInstaller
{
    /// <summary>
    /// Installs SwaggerGen services for generating API documentation.
    /// </summary>
    /// <param name="serviceCollection">The service collection to which SwaggerGen services will be added.</param>
    /// <param name="options">Options for dependency installation.</param>
    public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            // Configure Swagger documentation options
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = Constants.Service.ApiName,
                Version = "v1"
            });

            // Enable Swagger annotations
            c.EnableAnnotations();

            // Include XML comments in Swagger documentation if in development environment
            if (Infrastructure.Constants.Environment.IsDevelopment == false)
                return;

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var path = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(path);
        });
    }
}
