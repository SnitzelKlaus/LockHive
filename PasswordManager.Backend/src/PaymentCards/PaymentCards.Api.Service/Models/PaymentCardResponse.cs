using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace PasswordManager.PaymentCards.Api.Service.Models
{
    [SwaggerSchema(Nullable = false, Required = new[] { "id", "cardNumber", "expiryMonth", "expiryYear", "cvv" })]
    public sealed class PaymentCardResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; }

        [JsonPropertyName("cardHolderName")]
        public string CardHolderName { get; set; }

        [JsonPropertyName("expiryMonth")]
        public int ExpiryMonth { get; set; }

        [JsonPropertyName("expiryYear")]
        public int ExpiryYear { get; set; }

        [JsonPropertyName("cvv")]
        public string Cvv { get; set; }

        public PaymentCardResponse(Guid id, string cardNumber, string cardHolderName, int expiryMonth, int expiryYear, string cvv)
        {
            Id = id;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Cvv = cvv;
        }
    }
}
