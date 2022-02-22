namespace Trading_Engine.Application.Commands
{
    public class CancelOrReplaceActionCommand : ICommand
    {
        public string OrderId { get; private set; }
        public int Price { get; }
        public int Quantity { get; }

        public CancelOrReplaceActionCommand(string orderId, int price, int quantity)
        {
            OrderId = orderId;
            Price = price;
            Quantity = quantity;
        }
    }
}