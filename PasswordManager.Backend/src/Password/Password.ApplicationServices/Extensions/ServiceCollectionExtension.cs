using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Password.CreatePassword;
using PasswordManager.Password.ApplicationServices.Password.DeletePassword;
using PasswordManager.Password.ApplicationServices.Password.GetPassword;
using PasswordManager.Password.ApplicationServices.Password.UpdatePassword;
using PasswordManager.Password.ApplicationServices.PasswordGenerator;

namespace PasswordManager.Password.ApplicationServices.Extensions;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServiceServices(this IServiceCollection services)
    {
        //Add application service services
        //Use scoped as method to add services
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<ICreatePasswordService, CreatePasswordService>();
        services.AddScoped<IGetPasswordService, GetPasswordService>();
        services.AddScoped<IUpdatePasswordService, UpdatePasswordService>();
        services.AddScoped<IDeletePasswordService, DeletePasswordService>();
        services.AddScoped<IGenerateSecureKeyService, GenerateSecureKeyService>();

        return services;
    }
}
