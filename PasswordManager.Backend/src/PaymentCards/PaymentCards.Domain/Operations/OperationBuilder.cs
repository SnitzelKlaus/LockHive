using PasswordManager.PaymentCards.Domain.PaymentCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.PaymentCards.Domain.Operations
{
    public static class OperationBuilder
    {
        private static Operation CreateOperation(Guid securityKeyId, OperationName operationType, string createdBy, Dictionary<string, string>? data)
            => new(Guid.NewGuid(), Guid.NewGuid().ToString(), createdBy, securityKeyId, operationType, OperationStatus.Queued, DateTime.UtcNow, DateTime.UtcNow, null, data);

        public static Operation CreatePaymentCard(PaymentCardModel paymentCardModel, string createdBy)
        {
            var data = new Dictionary<string, string>
            {
                { OperationDataConstants.CreatePaymentCardUserId, paymentCardModel.UserId.ToString() },
                { OperationDataConstants.CreatePaymentCardCardNumber, paymentCardModel.CardNumber },
                { OperationDataConstants.CreatePaymentCardCardholderName, paymentCardModel.CardHolderName },
                { OperationDataConstants.CreatePaymentCardExpiryMonth, paymentCardModel.ExpiryMonth.ToString() },
                { OperationDataConstants.CreatePaymentCardExpiryYear, paymentCardModel.ExpiryYear.ToString() },
                { OperationDataConstants.CreatePaymentCardCvv, paymentCardModel.Cvv }
            };

            return CreateOperation(paymentCardModel.Id, OperationName.CreatePaymentCard, createdBy, data);
        }

        public static Operation UpdatePaymentCard(PaymentCardModel paymentCardModel, string createdBy)
        {
            var data = new Dictionary<string, string>
            {
                { OperationDataConstants.CurrentPaymentCardCardNumber, paymentCardModel.CardNumber },
                { OperationDataConstants.CurrentPaymentCardCardholderName, paymentCardModel.CardHolderName },
                { OperationDataConstants.CurrentPaymentCardExpiryMonth, paymentCardModel.ExpiryMonth.ToString() },
                { OperationDataConstants.CurrentPaymentCardExpiryYear, paymentCardModel.ExpiryYear.ToString() },
                { OperationDataConstants.CurrentPaymentCardCvv, paymentCardModel.Cvv },

                { OperationDataConstants.NewPaymentCardCardNumber, paymentCardModel.CardNumber },
                { OperationDataConstants.NewPaymentCardCardholderName, paymentCardModel.CardHolderName },
                { OperationDataConstants.NewPaymentCardExpiryMonth, paymentCardModel.ExpiryMonth.ToString() },
                { OperationDataConstants.NewPaymentCardExpiryYear, paymentCardModel.ExpiryYear.ToString() },
                { OperationDataConstants.NewPaymentCardCvv, paymentCardModel.Cvv }
            };

            return CreateOperation(paymentCardModel.Id, OperationName.UpdatePaymentCard, createdBy, data);
        }

        public static Operation DeletePaymentCard(Guid paymentCardId, string createdBy) 
            => CreateOperation(paymentCardId, OperationName.DeletePaymentCard, createdBy, null);
    }
}
