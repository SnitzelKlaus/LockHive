namespace PaymentCards.Messages.DeletePaymetCard
{
    public sealed class DeletePaymentCardCommand : AbstractRequestAcceptedCommand
    {
        public DeletePaymentCardCommand(string requestId) : base(requestId)
        {
        }
    }
}
