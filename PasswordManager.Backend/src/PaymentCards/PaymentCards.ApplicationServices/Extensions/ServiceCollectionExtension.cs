using Microsoft.Extensions.DependencyInjection;
using PasswordManager.PaymentCards.ApplicationServices.Operations;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.CreatePaymentCard;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.DeletePaymentCard;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.GetPaymentCard;
using PasswordManager.PaymentCards.ApplicationServices.PaymentCard.UpdatePaymentCard;

namespace PasswordManager.PaymentCards.ApplicationServices.Extensions;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServiceServices(this IServiceCollection services)
    {
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<ICreatePaymentCardService, CreatePaymentCardService>();
        services.AddScoped<IUpdatePaymentCardService, UpdatePaymentCardService>();
        services.AddScoped<IDeletePaymentCardService, DeletePaymentCardService>();
        services.AddScoped<IGetPaymentCardService, GetPaymentCardService>();

        return services;
    }
}
