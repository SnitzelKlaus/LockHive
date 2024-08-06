using Microsoft.AspNetCore.Mvc.Infrastructure;
using PasswordManager.PaymentCards.Domain.Operations;
using PasswordManager.PaymentCards.Domain.PaymentCards;

namespace PaymentCards.Worker.Service.CreatePaymentCard
{
    internal class CreatePaymentCardOperationHelper
    {
        internal static PaymentCardModel Map(Guid paymentCardId, Operation operation)
            => new PaymentCardModel(
                paymentCardId, 
                GetPaymentCardUserId(operation), 
                GetPaymentCardCardNumber(operation), 
                GetPaymentCardCardHolderName(operation), 
                GetPaymentCardExpiryMonth(operation),
                GetPaymentCardExpiryYear(operation),
                GetPaymentCardCvv(operation)
                );

        private static string GetPaymentCardOperationData(Operation operation, string operationDataConstant)
        {
            if (operation.Data is null || operation.Data.TryGetValue(operationDataConstant, out var getPaymentCardOperationData) is false)
                throw new InvalidOperationException($"Could not find PaymentCard: {operationDataConstant}, in operation with request id: {operation.RequestId}");

            return getPaymentCardOperationData;
        }

        private static Guid GetPaymentCardUserId(Operation operation)
        {
            var userId = GetPaymentCardOperationData(operation, OperationDataConstants.CreatePaymentCardUserId);

            if (Guid.TryParse(userId, out var userIdGuid) is false)
                throw new InvalidOperationException($"Could not parse userId: {userId} to Guid, in operation with request id: {operation.RequestId}");

            return userIdGuid;
        }

        private static string GetPaymentCardCardNumber(Operation operation)
            => GetPaymentCardOperationData(operation, OperationDataConstants.CreatePaymentCardCardNumber);

        private static string GetPaymentCardCardHolderName(Operation operation)
            => GetPaymentCardOperationData(operation, OperationDataConstants.CreatePaymentCardCardholderName);

        private static int GetPaymentCardExpiryMonth(Operation operation)
        {
            var expiryMonth = GetPaymentCardOperationData(operation, OperationDataConstants.CreatePaymentCardExpiryMonth);

            if (int.TryParse(expiryMonth, out var expiryMonthInt) is false)
                throw new InvalidOperationException($"Could not parse expiryMonth: {expiryMonth} to int, in operation with request id: {operation.RequestId}");

            return expiryMonthInt;
        }

        private static int GetPaymentCardExpiryYear(Operation operation)
        {
            var expiryYear = GetPaymentCardOperationData(operation, OperationDataConstants.CreatePaymentCardExpiryYear);

            if (int.TryParse(expiryYear, out var expiryYearInt) is false)
                throw new InvalidOperationException($"Could not parse expiryYear: {expiryYear} to int, in operation with request id: {operation.RequestId}");

            return expiryYearInt;
        }

        private static string GetPaymentCardCvv(Operation operation)
            => GetPaymentCardOperationData(operation, OperationDataConstants.CreatePaymentCardCvv);
    }
}