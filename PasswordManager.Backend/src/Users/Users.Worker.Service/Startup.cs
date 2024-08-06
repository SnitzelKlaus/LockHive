using Rebus.Config;

namespace PasswordManager.Users.Worker.Service;
/// <summary>
/// Configures the application's services and HTTP request pipeline.
/// </summary>
public class Startup
{
    /// <summary>
    /// Gets the configuration of the application.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration of the application.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    /// <param name="services">The collection of services to be configured.</param>
    public void ConfigureServices(IServiceCollection services)
    {

    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The web hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.StartRebus();
    }
}
