namespace PaymentCards.Messages.UpdatePaymentCard
{
    public sealed class UpdatePaymentCardCommand : AbstractRequestAcceptedCommand
    {
        public UpdatePaymentCardCommand(string requestId) : base(requestId)
        {
        }
    }
}
