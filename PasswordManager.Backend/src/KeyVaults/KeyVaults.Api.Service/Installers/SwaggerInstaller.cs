using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PasswordManager.KeyVaults.Api.Service.Installers;

public class SwaggerInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = Constants.Services.ApiName,
                Version = "v1"
            });

            c.EnableAnnotations();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var path = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(path);
        });
    }
}
