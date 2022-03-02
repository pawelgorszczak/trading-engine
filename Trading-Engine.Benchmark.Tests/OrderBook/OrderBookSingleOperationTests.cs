using BenchmarkDotNet.Attributes;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class OrderBookSingleOperationTests
    {
        // prepared dataset
        private List<Order> buyOrderList;

        private int buyOrdersCount = 1000000;
        
        // max price
        private int price;

        [ParamsSource(nameof(OrderBooks))]
        public IOrderBookTest OrderBookParam { get; set; }

        // public property
        public IEnumerable<IOrderBookTest> OrderBooks => new IOrderBookTest[] { new OrderBook1(), new OrderBook2(), new OrderBook3() };

        public OrderBookSingleOperationTests()
        {
            buyOrderList = new List<Order>();
            price = 0;
            for (int i = 0; i < buyOrdersCount; i++)
            {
                price += 2;
                buyOrderList.Add(new Order("Buy_OrderId" + i, price, 20));
            }
            buyOrderList.Reverse();

        }

        [IterationSetup]
        public void Setup()
        {
            PrepareDataSet(OrderBookParam);
        }

        [Benchmark]
        public void AddOrder_AtTheBeginning()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", price + 2, 20), SideType.Buy);
        }

        [Benchmark]
        public void AddOrder_AtTheEnd()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", 1, 20), SideType.Buy);
        }

        [Benchmark]
        public void AddOrder_InTheMiddle()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", (price / 2) + 1, 20), SideType.Buy);
        }

        [Benchmark]
        public void AddOrder_InTheMiddleDuplicated()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", price / 2, 20), SideType.Buy);
        }

        [Benchmark]
        public void RemoveOrder_AtTheBeginning()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId0");
        }

        [Benchmark]
        public void RemoveOrder_AtTheEnd()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (buyOrdersCount - 1));
        }

        [Benchmark]
        public void RemoveOrder_InTheMiddle()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (buyOrdersCount / 2));
        }

        // remove test when not found in first list of buys

        private void PrepareDataSet(IOrderBookTest orderBook)
        {            
            orderBook.InidializeBuyOrders(buyOrderList);
        }
    }
}
