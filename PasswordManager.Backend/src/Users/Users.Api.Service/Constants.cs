namespace PasswordManager.Users.Api.Service;
/// <summary>
/// Contains constants related to the API service.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Contains route-related constants.
    /// </summary>
    public static class Routes
    {
        public const string SwaggerEndpoint = "/swagger/v1/swagger.json";
    }

    /// <summary>
    /// Contains constants related to services.
    /// </summary>
    public static class Services
    {
        public static string ApiName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName} API";
    }

    /// <summary>
    /// Contains constants related to the API service.
    /// </summary>
    public static class Service
    {
        public static string ApiName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName} API";
        public static string FullyQualifiedName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName}.Api.Service";
        public static string ApplicationType => "Api.Service";
    }
}
