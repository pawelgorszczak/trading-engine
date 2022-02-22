using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class FillOrKillOrderCommand : ICommand
    {
        public string OrderId { get; }
        public int Price { get; }
        public SideType SideType { get; }
        public int Quantity { get; }

        public FillOrKillOrderCommand(string orderId, int price, SideType sideType, int quantity)
        {
            OrderId = orderId;
            Price = price;
            SideType = sideType;
            Quantity = quantity;
        }
    }
}