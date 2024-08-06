namespace PaymentCards.Messages.CreatePaymentCard
{
    public sealed class CreatePaymentCardCommand : AbstractRequestAcceptedCommand
    {
        public CreatePaymentCardCommand(string requestId) : base(requestId)
        {
        }
    }
}
