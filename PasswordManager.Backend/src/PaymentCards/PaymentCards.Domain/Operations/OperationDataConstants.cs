using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.PaymentCards.Domain.Operations
{
    public sealed class OperationDataConstants
    {
        #region CreatePaymentCard
        public const string CreatePaymentCardUserId = "createPaymentCardUserId";
        public const string CreatePaymentCardCardNumber = "createPaymentCardCardNumber";
        public const string CreatePaymentCardCardholderName = "createPaymentCardCardholderName";
        public const string CreatePaymentCardExpiryMonth = "createPaymentCardExpiryMonth";
        public const string CreatePaymentCardExpiryYear = "createPaymentCardExpiryYear";
        public const string CreatePaymentCardCvv = "createPaymentCardCvv";
        #endregion

        #region UpdatePaymentCard
        public const string CurrentPaymentCardCardNumber = "currentPaymentCardCardNumber";
        public const string CurrentPaymentCardCardholderName = "currentPaymentCardCardholderName";
        public const string CurrentPaymentCardExpiryMonth = "currentPaymentCardExpiryDate";
        public const string CurrentPaymentCardExpiryYear = "currentPaymentCardExpiryYear";
        public const string CurrentPaymentCardCvv = "currentPaymentCardCvv";

        public const string NewPaymentCardCardNumber = "newPaymentCardCardNumber";
        public const string NewPaymentCardCardholderName = "newPaymentCardCardholderName";
        public const string NewPaymentCardExpiryMonth = "newPaymentCardExpiryMonth";
        public const string NewPaymentCardExpiryYear = "newPaymentCardExpiryYear";
        public const string NewPaymentCardCvv = "newPaymentCardCvv";
        #endregion
    }
}
