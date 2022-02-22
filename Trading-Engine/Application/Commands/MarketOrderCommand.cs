using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class MarketOrderCommand : ICommand
    {
        public string OrderId { get; }
        public SideType SideType { get; }
        public int Quantity { get; }

        public MarketOrderCommand(string orderId, SideType sideType, int quantity)
        {
            OrderId = orderId;
            SideType = sideType;
            Quantity = quantity;
        }
    }
}