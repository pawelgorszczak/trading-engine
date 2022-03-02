using BenchmarkDotNet.Attributes;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    /*
        |                         Method | OrderBookParam |          Mean |         Error |        StdDev |            Min |           Max |        Median |
        |------------------------------- |--------------- |--------------:|--------------:|--------------:|---------------:|--------------:|--------------:|
        |        AddOrder_AtTheBeginning |     OrderBook1 | 252,230.33 us | 29,540.853 us | 19,539.445 us | 232,704.750 us | 287,613.85 us | 245,526.35 us |
        |              AddOrder_AtTheEnd |     OrderBook1 | 139,547.69 us |  4,484.480 us |  2,966.206 us | 135,779.200 us | 143,079.30 us | 138,999.05 us |
        |           AddOrder_InTheMiddle |     OrderBook1 | 185,680.70 us |  3,133.545 us |  2,072.646 us | 181,826.850 us | 188,268.55 us | 186,381.60 us |
        | AddOrder_InTheMiddleDuplicated |     OrderBook1 | 179,552.26 us |  4,465.007 us |  2,657.054 us | 176,535.100 us | 184,861.50 us | 179,393.70 us |
        |     RemoveOrder_AtTheBeginning |     OrderBook1 |  22,145.57 us |  1,416.935 us |    937.215 us |  21,052.300 us |  23,946.20 us |  22,051.00 us |
        |           RemoveOrder_AtTheEnd |     OrderBook1 |     936.66 us |    150.819 us |     99.758 us |     802.450 us |   1,068.65 us |     954.95 us |
        |        RemoveOrder_InTheMiddle |     OrderBook1 |  16,625.88 us |  6,772.805 us |  4,479.791 us |  12,580.200 us |  25,800.60 us |  15,015.95 us |
        |        AddOrder_AtTheBeginning |     OrderBook2 | 503,919.94 us | 14,004.642 us |  7,324.699 us | 495,494.400 us | 514,436.20 us | 501,180.75 us |
        |              AddOrder_AtTheEnd |     OrderBook2 | 263,882.63 us | 16,481.816 us | 10,901.701 us | 251,946.700 us | 281,894.30 us | 260,551.75 us |
        |           AddOrder_InTheMiddle |     OrderBook2 | 378,926.27 us |  3,685.605 us |  2,437.800 us | 375,674.200 us | 382,697.40 us | 378,056.80 us |
        | AddOrder_InTheMiddleDuplicated |     OrderBook2 | 383,219.40 us |  2,369.803 us |  1,239.453 us | 381,055.500 us | 385,148.50 us | 383,318.25 us |
        |     RemoveOrder_AtTheBeginning |     OrderBook2 |  21,803.33 us |  1,407.767 us |    931.151 us |  20,776.100 us |  23,398.00 us |  21,721.35 us |
        |           RemoveOrder_AtTheEnd |     OrderBook2 |     906.18 us |     87.482 us |     52.059 us |     809.350 us |     974.15 us |     902.35 us |
        |        RemoveOrder_InTheMiddle |     OrderBook2 |  14,113.72 us |  3,944.665 us |  2,609.151 us |  11,863.600 us |  18,767.60 us |  13,095.05 us |
        |        AddOrder_AtTheBeginning |     OrderBook3 |      13.87 us |      7.652 us |      5.062 us |       8.700 us |      20.20 us |      11.90 us |
        |              AddOrder_AtTheEnd |     OrderBook3 |   9,990.91 us |    370.909 us |    245.333 us |   9,629.250 us |  10,314.65 us |  10,021.90 us |
        |           AddOrder_InTheMiddle |     OrderBook3 |   5,107.89 us |    303.143 us |    158.550 us |   4,895.500 us |   5,296.00 us |   5,083.20 us |
        | AddOrder_InTheMiddleDuplicated |     OrderBook3 |   5,355.94 us |    427.967 us |    254.676 us |   4,996.100 us |   5,605.20 us |   5,496.50 us |
        |     RemoveOrder_AtTheBeginning |     OrderBook3 |  26,388.79 us |  2,083.249 us |  1,377.940 us |  24,998.300 us |  28,957.30 us |  26,236.95 us |
        |           RemoveOrder_AtTheEnd |     OrderBook3 |      19.44 us |      6.905 us |      4.567 us |      14.000 us |      25.10 us |      18.25 us |
        |        RemoveOrder_InTheMiddle |     OrderBook3 |  15,283.77 us |  1,457.737 us |    964.202 us |  13,827.800 us |  16,600.80 us |  15,317.65 us |
     */
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
