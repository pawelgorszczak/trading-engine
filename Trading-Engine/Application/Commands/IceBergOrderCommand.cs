using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class IceBergOrderCommand : ICommand
    {
        public string OrderId { get; }
        public int Price { get; }
        public SideType SideType { get; }
        public int Quantity { get; }

        public int DisplaySize { get; }

        public IceBergOrderCommand(string orderId, int price, SideType sideType, int quantity, int displaySize)
        {
            OrderId = orderId;
            Price = price;
            SideType = sideType;
            Quantity = quantity;
            DisplaySize = displaySize;
        }
    }
}