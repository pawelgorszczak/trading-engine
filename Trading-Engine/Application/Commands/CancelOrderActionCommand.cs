namespace Trading_Engine.Application.Commands
{
    public class CancelOrderActionCommand : ICommand
    {
        public string OrderId { get; private set; }

        public CancelOrderActionCommand(string orderId)
        {
            OrderId = orderId;
        }
    }
}