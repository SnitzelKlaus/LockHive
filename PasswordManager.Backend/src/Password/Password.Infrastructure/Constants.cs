namespace PasswordManager.Password.Infrastructure;
public static class Constants
{
    public static class Service
    {
        public const string ServiceName = "Password";
        public const string BoundedContext = "PasswordManager";
    }

    public static class Environment
    {
        public static bool IsDevelopment
        {
            get
            {
                var variable =
                    System.Environment.GetEnvironmentVariable(ConfigurationKeys.AspnetCoreDevelopmentEnvironment);
                return string.IsNullOrEmpty(variable) == false && variable.Equals("Development");
            }
        }

        public static bool IsGeneratingApi
        {
            get
            {
                var variable =
                    System.Environment.GetEnvironmentVariable(ConfigurationKeys.AspnetCoreDevelopmentEnvironment);
                return string.IsNullOrEmpty(variable) == false && variable.Equals("SwaggerGen");
            }
        }
    }

    public static class ServiceBus
    {
        public static string InputQueue => $"{Service.BoundedContext}_{Service.ServiceName}_input".ToLower();
    }

    public static class ConfigurationKeys
    {
        public const string AspnetCoreDevelopmentEnvironment = "ASPNETCORE_ENVIRONMENT";
        public const string ServiceBusConnectionString = "ServiceBusConnectionString";
        public const string SqlDbConnectionString = "SqlDbConnectionString";
    }
}
