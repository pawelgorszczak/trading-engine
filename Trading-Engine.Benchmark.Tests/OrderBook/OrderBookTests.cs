using BenchmarkDotNet.Attributes;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    //https://arthurminduca.com/2016/04/25/choosing-the-right-collection/
    [SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 5)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class OrderBookTests
    {
        [Benchmark]
        public void AddBuyAndSellOrder1()
        {
            AddBuyAndSellOrderSimpleScenario(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyAndSellOrder2()
        {
            AddBuyAndSellOrderSimpleScenario(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyAndSellOrder3()
        {
            AddBuyAndSellOrderSimpleScenario(new OrderBook3());
        }

        [Benchmark]
        public void AddBuyOrderComplexOrder1()
        {
            AddBuyOrderComplexScenrio(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyOrderComplexOrder2()
        {
            AddBuyOrderComplexScenrio(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyOrderComplexOrder3()
        {
            AddBuyOrderComplexScenrio(new OrderBook3());
        }

        [Benchmark]
        public void AddBuyOrdersIntoBigList()
        {
            AddBuyOrdersIntoBigList(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyOrders2IntoBigList()
        {
            AddBuyOrdersIntoBigList(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyOrders3IntoBigList()
        {
            AddBuyOrdersIntoBigList(new OrderBook3());
        }

        /*
            |              Method |     Mean |    Error |   StdDev |      Min |      Max |   Median |
            |-------------------- |---------:|---------:|---------:|---------:|---------:|---------:|
            | AddBuyAndSellOrder1 | 757.5 ns | 43.87 ns | 11.39 ns | 749.0 ns | 775.8 ns | 751.3 ns |
            | AddBuyAndSellOrder2 | 368.4 ns |  8.48 ns |  2.20 ns | 365.2 ns | 370.4 ns | 368.4 ns |
            | AddBuyAndSellOrder3 | 363.6 ns |  5.27 ns |  1.37 ns | 362.2 ns | 365.6 ns | 363.3 ns |
         */
        private void AddBuyAndSellOrderSimpleScenario(IOrderBookTest orderBook)
        {
            var buyOrder = new Order("YuFU", 13, 100);
            var sellOrder = new Order("IpD8", 14, 150);

            orderBook.AddOrder(buyOrder, SideType.Buy);
            orderBook.AddOrder(sellOrder, SideType.Sell);
        }

        /*
            |                   Method |     Mean |     Error |    StdDev |      Min |      Max |   Median |
            |------------------------- |---------:|----------:|----------:|---------:|---------:|---------:|
            | AddBuyOrderComplexOrder1 | 7.084 us | 0.4749 us | 0.0735 us | 6.974 us | 7.131 us | 7.115 us |
            | AddBuyOrderComplexOrder2 | 5.845 us | 0.2177 us | 0.0565 us | 5.775 us | 5.926 us | 5.840 us |
            | AddBuyOrderComplexOrder3 | 3.211 us | 0.0214 us | 0.0056 us | 3.202 us | 3.217 us | 3.212 us |
         */
        private void AddBuyOrderComplexScenrio(IOrderBookTest orderBook)
        {
            // having buy store: 112, 80, 79, 50, 50, 50, 40, 20, 15, 10
            var buyOrderList = new List<Order>()
            {
                new Order("YuFU", 112, 20),
                new Order("YuFU", 80, 30),
                new Order("YuFU", 79, 40),
                new Order("YuFU", 50, 50),
                new Order("YuFU", 50, 60),
                new Order("YuFU", 50, 70),
                new Order("YuFU", 40, 80),
                new Order("YuFU", 20, 90),
                new Order("YuFU", 15, 90),
                new Order("YuFU", 10, 70),
            };
            orderBook.InidializeBuyOrders(buyOrderList);

            // add 200, 190, 80, 10, 14, 50, 45
            orderBook.AddOrder(new Order("YuFU", 200, 20), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 190, 30), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 80, 40), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 10, 50), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 14, 40), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 50, 30), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 45, 20), SideType.Buy);
        }

        /*
             |                   Method |       Mean |    Error |   StdDev |        Min |        Max |     Median |
             |------------------------- |-----------:|---------:|---------:|-----------:|-----------:|-----------:|
             |  AddBuyOrdersIntoBigList |   946.0 ms | 57.52 ms | 14.94 ms |   927.3 ms |   961.5 ms |   948.5 ms |
             | AddBuyOrders2IntoBigList | 1,715.9 ms | 54.87 ms | 14.25 ms | 1,698.4 ms | 1,735.3 ms | 1,719.3 ms |
             | AddBuyOrders3IntoBigList |   348.6 ms | 37.31 ms |  5.77 ms |   341.2 ms |   355.2 ms |   349.1 ms |
         */
        private void AddBuyOrdersIntoBigList(IOrderBookTest orderBook)
        {
            var buyOrderList = new List<Order>();
            var price = 0;
            for (int i = 0; i < 1000000; i++)
            {
                price += 2;
                buyOrderList.Add(new Order("YuFU", price, 20));                
            }
            buyOrderList.Reverse();
            orderBook.InidializeBuyOrders(buyOrderList);

            // add at beginning
            orderBook.AddOrder(new Order("YuFU", price + 2, 20), SideType.Buy);

            // add at the end
            orderBook.AddOrder(new Order("YuFU", 1, 20), SideType.Buy);

            // add in the middle
            orderBook.AddOrder(new Order("YuFU", (price/2) + 1, 20), SideType.Buy);

            // add duplicate in the middle
            orderBook.AddOrder(new Order("YuFU", price / 2, 20), SideType.Buy);
        }
    }
}
