namespace PasswordManager.Password.Api.Service;

public static class Constants
{
    public static class Routes
    {
        public const string SwaggerEndpoint = "/swagger/v1/swagger.json";
    }

    public static class Services
    {
        public static string ApiName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName} API";
    }

    public static class Service
    {
        public static string ApiName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName} API";
        public static string FullyQualifiedName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName}.Api.Service";
        public static string ApplicationType => "Api.Service";
    }
}
