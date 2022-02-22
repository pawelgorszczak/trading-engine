using Trading_Engine.Application;
using Trading_Engine.Application.Commands;
using Trading_Engine.Domain;

public class IceBergOrderCommandHandler : ICommandHandler<IceBergOrderCommand>
{
    private readonly OrderBook _orderBook;

    public IceBergOrderCommandHandler(OrderBook orderBook)
    {
        _orderBook = orderBook;
    }
    public string Handle(IceBergOrderCommand command)
    {
        var nextOrder = GetNextOrder(command.SideType, command.Price);
        var orderLeftTohandle = command.Quantity;
        int operationCost = 0;


        return operationCost.ToString();
    }

    private Order GetNextOrder(SideType sideType, int pricePoint)
    {
        var nextOrder = sideType == SideType.Buy ? _orderBook.SellOrders.FirstOrDefault() : _orderBook.BuyOrders.FirstOrDefault();
        if (nextOrder == null) return null;

        if ((sideType == SideType.Buy && nextOrder.Price <= pricePoint) || (sideType == SideType.Sell && nextOrder.Price >= pricePoint))
        {
            return nextOrder;
        }
        return null;
    }
}
