using Trading_Engine.Application;
using Trading_Engine.Domain;

public class LimitOrderCommand : ICommand
{
    public string OrderId { get; }
    public int Price { get; }
    public SideType SideType { get; }
    public int Quantity { get; }

    public LimitOrderCommand(string orderId, int price, SideType sideType, int quantity)
    {
        OrderId = orderId;
        Price = price;
        SideType = sideType;
        Quantity = quantity;
    }
}
