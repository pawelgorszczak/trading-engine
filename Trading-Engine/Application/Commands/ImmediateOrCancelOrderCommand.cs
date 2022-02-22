using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class ImmediateOrCancelOrderCommand : ICommand
    {
        public string OrderId { get; }
        public int Price { get; }
        public SideType SideType { get; }
        public int Quantity { get; }

        public ImmediateOrCancelOrderCommand(string orderId, int price, SideType sideType, int quantity)
        {
            OrderId = orderId;
            Price = price;
            SideType = sideType;
            Quantity = quantity;
        }
    }
}