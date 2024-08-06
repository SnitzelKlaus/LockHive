namespace PasswordManager.Users.Worker.Service;
/// <summary>
/// Provides constants used within the worker service application.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Provides constants related to the service.
    /// </summary>
    public static class Service
    {
        public static string FullyQualifiedName => $"{Infrastructure.Constants.Service.BoundedContext}.{Infrastructure.Constants.Service.ServiceName}.Worker.Service";
        public static string ApplicationType => "Worker.Service";
    }

    /// <summary>
    /// Provides constants related to the service bus.
    /// </summary>
    public static class ServiceBus
    {
        public static string InputQueue => $"{Infrastructure.Constants.Service.BoundedContext}_{Infrastructure.Constants.Service.ServiceName}_input".ToLower();
    }

    /// <summary>
    /// Provides constants related to Rebus messaging.
    /// </summary>
    public static class Rebus
    {
        public const string OperationId = "current-operation-id";
    }
}
