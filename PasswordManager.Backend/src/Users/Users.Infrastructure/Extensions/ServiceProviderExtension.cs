using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PasswordManager.Users.Infrastructure.Installers;

namespace PasswordManager.Users.Infrastructure.Extensions;
/// <summary>
/// Provides extension methods for IServiceCollection to facilitate automatic service registration from assemblies.
/// </summary>
public static class ServiceProviderExtension
{
    /// <summary>
    /// Automatically scans and registers services from the specified assembly type's assembly using custom dependency installers.
    /// </summary>
    /// <typeparam name="TAssemblyType">The type from which to derive the assembly for scanning.</typeparam>
    /// <param name="self">The IServiceCollection instance.</param>
    /// <param name="options">Options for dependency installation.</param>
    public static void AddFromAssembly<TAssemblyType>(this IServiceCollection self, DependencyInstallerOptions options)
    {
        if (self == null)
            throw new ArgumentNullException(nameof(self));

        var assemblyToRegister = GetAssembly<TAssemblyType>();

        RegisterAssembly(self, assemblyToRegister, options);
    }

    /// <summary>
    /// Retrieves the assembly associated with the specified type.
    /// </summary>
    /// <typeparam name="THandler">The type whose assembly is to be retrieved.</typeparam>
    /// <returns>The assembly containing the specified type.</returns>
    private static Assembly GetAssembly<THandler>()
    {
        return typeof(THandler).Assembly;
    }

    /// <summary>
    /// Registers services from a given assembly using defined dependency installers within the assembly.
    /// </summary>
    /// <param name="services">The IServiceCollection instance for registering services.</param>
    /// <param name="assemblyToRegister">The assembly from which to register services.</param>
    /// <param name="options">Options for dependency installation.</param>
    private static void RegisterAssembly(IServiceCollection services, Assembly assemblyToRegister, DependencyInstallerOptions options)
    {
        var typesToAutoRegister = assemblyToRegister.GetTypes()
            .Where(IsDependencyInstaller)
            .ToList();

        foreach (var type in typesToAutoRegister)
        {
            var instance = Activator.CreateInstance(type);
            var typedInstance = instance as IDependencyInstaller;
            typedInstance?.Install(services, options);
        }
    }

    /// <summary>
    /// Determines if a type is a dependency installer.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type implements IDependencyInstaller and is not an interface or abstract; otherwise, false.</returns>
    private static bool IsDependencyInstaller(Type type)
    {
        return !type.IsInterface
               && !type.IsAbstract
               && typeof(IDependencyInstaller).IsAssignableFrom(type);
    }
}
