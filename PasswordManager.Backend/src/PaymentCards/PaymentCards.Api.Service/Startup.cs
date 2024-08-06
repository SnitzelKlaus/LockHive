using System.Text.Json.Serialization;
using PasswordManager.PaymentCards.ApplicationServices.Extensions;
using PasswordManager.PaymentCards.Infrastructure.Extensions;

namespace PasswordManager.PaymentCards.Api.Service;

public class Startup
{
    public IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //Figure out if AddMvc is used
        services.AddMvc()
            .AddJsonOptions(options =>
            {
                var enumConvertor = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(enumConvertor);
            });

        services.AddApplicationServiceServices();

        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.EnableAnnotations();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging())
            app.UseDeveloperExceptionPage();

        app.UseSwagger();

        if (env.IsDevelopment() || env.IsStaging() || Infrastructure.Constants.Environment.IsGeneratingApi)
            app.UseDeveloperExceptionPage();

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint(Constants.Routes.SwaggerEndpoint, Constants.Services.ApiName);
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });

        if (env.IsEnvironment("integration-test") || Infrastructure.Constants.Environment.IsGeneratingApi)
            return;

        app.EnsureDatabaseMigrated();
        //If you want to add rebus you need to add it here!
    }
}
