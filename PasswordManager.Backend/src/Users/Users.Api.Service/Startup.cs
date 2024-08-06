using System.Security.Claims;
using System.Text.Json.Serialization;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using PasswordManager.Users.Api.Service.CurrentUser;
using PasswordManager.Users.Api.Service.Handlers;
using PasswordManager.Users.ApplicationServices.Extensions;
using PasswordManager.Users.Infrastructure.Extensions;

namespace PasswordManager.Users.Api.Service;

/// <summary>
/// Configures services and the HTTP request pipeline during application startup.
/// </summary>
public class Startup
{
    public IConfiguration Configuration { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add MVC services
        services.AddMvc()
            .AddJsonOptions(options =>
            {
                var enumConvertor = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(enumConvertor);
            });

        // Add application services
        services.AddApplicationServiceServices();

        // Add controllers
        services.AddControllers();

        // Add Swagger generation
        services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new string[] {}
                }
            });
        });

        // Add Firebase authentication
        services.AddSingleton(FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(Configuration[Infrastructure.Constants.ConfigurationKeys.FirebaseProjectCredentials]),
        }));
        services.AddHttpContextAccessor();
        services.AddScoped<ClaimsPrincipal>(p => p.GetRequiredService<IHttpContextAccessor>().HttpContext?.User);
        services
            .AddAuthentication("FirebaseUser")
            .AddScheme<AuthenticationSchemeOptions, FirebaseUserAuthenticationHandler>("FirebaseUser",
                (o) =>
                {

                });

        // Add current user service
        services.AddTransient<ICurrentUser, CurrentUser.CurrentUser>();

    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging())
            app.UseDeveloperExceptionPage();

        // Enable Swagger UI
        app.UseSwagger();
        if (env.IsDevelopment() || env.IsStaging() || Infrastructure.Constants.Environment.IsGeneratingApi)
            app.UseDeveloperExceptionPage();

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint(Constants.Routes.SwaggerEndpoint, Constants.Services.ApiName);
        });

        app.UseRouting();
        
        app.UseAuthorization();
        app.UseAuthentication();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });

        // Ensure database migration
        if (env.IsEnvironment("integration-test") || Infrastructure.Constants.Environment.IsGeneratingApi)
            return;

        app.EnsureDatabaseMigrated();
    }
}
