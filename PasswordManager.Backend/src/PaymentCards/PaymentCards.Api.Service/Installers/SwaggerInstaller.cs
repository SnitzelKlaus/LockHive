using PasswordManager.PaymentCards.Infrastructure.Installers;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PasswordManager.PaymentCards.Api.Service.Installers;

public class SwaggerInstaller : IDependencyInstaller
{
    public void Install(IServiceCollection serviceCollection, DependencyInstallerOptions options)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = Constants.Service.ApiName,
                Version = "v1"
            });

            c.EnableAnnotations();

            if (Infrastructure.Constants.Environment.IsDevelopment == false)
                return;

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var path = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(path);
        });
    }
}
