using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    /*
        |                         Method |               Categories | OrderBookParam |           Mean |         Error |       StdDev |         Median |            Min |           Max |
        |------------------------------- |------------------------- |--------------- |---------------:|--------------:|-------------:|---------------:|---------------:|--------------:|
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook1 | 232,964.070 us |  5,701.451 us | 3,771.157 us | 232,837.550 us | 227,733.200 us | 240,464.90 us |
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook2 | 523,625.210 us |  7,359.188 us | 4,867.647 us | 523,487.800 us | 515,879.400 us | 532,615.60 us |
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook3 |      17.550 us |      7.380 us |     4.881 us |      20.100 us |      11.200 us |      22.50 us |
        |                                |                          |                |                |               |              |                |                |               |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook1 | 148,720.350 us |  3,476.691 us | 2,299.616 us | 147,605.050 us | 146,514.100 us | 152,907.70 us |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook2 | 281,105.088 us |    773.428 us |   404.518 us | 281,158.100 us | 280,288.100 us | 281,715.30 us |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook3 |  10,552.000 us |    886.163 us |   527.341 us |  10,373.300 us |   9,917.400 us |  11,581.70 us |
        |                                |                          |                |                |               |              |                |                |               |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook1 | 245,295.763 us |  6,015.827 us | 3,146.394 us | 247,154.650 us | 239,745.800 us | 247,723.10 us |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook2 | 536,544.260 us |  5,747.917 us | 3,801.891 us | 535,990.500 us | 528,973.800 us | 543,229.10 us |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook3 |      18.140 us |     10.081 us |     6.668 us |      19.450 us |      10.200 us |      30.70 us |
        |                                |                          |                |                |               |              |                |                |               |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook1 | 242,817.740 us |  2,878.428 us | 1,903.902 us | 242,883.950 us | 240,581.200 us | 246,635.80 us |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook2 | 529,105.787 us | 14,685.427 us | 7,680.762 us | 526,781.900 us | 520,752.650 us | 543,497.75 us |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook3 |      17.289 us |      8.347 us |     4.967 us |      20.400 us |      10.400 us |      21.10 us |
        |                                |                          |                |                |               |              |                |                |               |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook1 |     892.094 us |     10.060 us |     5.986 us |     894.450 us |     878.850 us |     898.25 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook1 |  17,137.840 us |  2,785.951 us | 1,842.734 us |  16,259.550 us |  15,618.200 us |  20,781.80 us |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook2 |     981.740 us |    146.246 us |    96.732 us |     961.100 us |     871.750 us |   1,123.85 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook2 |  16,138.975 us |    934.966 us |   489.005 us |  16,081.650 us |  15,510.100 us |  17,057.40 us |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook3 |      12.320 us |      7.765 us |     5.136 us |      12.600 us |       6.500 us |      18.10 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook3 |       7.880 us |      7.831 us |     5.180 us |       7.850 us |       2.800 us |      13.70 us |
        |                                |                          |                |                |               |              |                |                |               |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook1 |  26,646.778 us |    668.081 us |   397.564 us |  26,630.900 us |  26,177.900 us |  27,322.60 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook1 |  42,693.180 us |  1,028.917 us |   680.565 us |  42,531.550 us |  41,940.400 us |  44,015.50 us |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook2 |  28,087.111 us |  2,324.168 us | 1,383.075 us |  27,678.700 us |  26,611.400 us |  30,253.90 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook2 |  46,166.275 us |  7,690.639 us | 4,022.353 us |  44,103.100 us |  42,695.500 us |  53,545.10 us |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook3 |       8.170 us |      7.892 us |     5.220 us |       8.300 us |       2.750 us |      13.35 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook3 |      13.020 us |      7.239 us |     4.788 us |      16.300 us |       7.150 us |      17.25 us |
        |                                |                          |                |                |               |              |                |                |               |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook1 |  17,320.240 us |  3,614.747 us | 2,390.931 us |  16,718.900 us |  15,225.600 us |  22,209.70 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook1 |  30,426.220 us |  1,094.436 us |   723.901 us |  30,427.050 us |  29,659.850 us |  31,646.65 us |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook2 |  15,344.312 us |    280.548 us |   146.732 us |  15,283.650 us |  15,258.650 us |  15,686.45 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook2 |  37,665.900 us | 10,030.131 us | 6,634.310 us |  37,386.200 us |  28,808.400 us |  48,103.00 us |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook3 |      14.150 us |      7.513 us |     4.471 us |      16.250 us |       6.250 us |      17.75 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook3 |      13.790 us |      8.885 us |     5.877 us |      13.750 us |       6.500 us |      25.90 us |
     */
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class OrderBookSingleOperationTests
    {
        // prepared dataset
        private List<Order> buyOrderList;

        // prepared dataset
        private List<Order> sellOrderList;

        private int ordersCount = 1000000;

        // max buy price
        private int maxBuyPrice;

        // max sell price
        private int maxSellPrice;

        [ParamsSource(nameof(OrderBooks))]
        public IOrderBookTest OrderBookParam { get; set; }

        // public property
        public IEnumerable<IOrderBookTest> OrderBooks => new IOrderBookTest[] { new OrderBook1(), new OrderBook2(), new OrderBook3() };

        public OrderBookSingleOperationTests()
        {
            buyOrderList = new List<Order>();
            maxBuyPrice = 0;
            for (int i = 0; i < ordersCount; i++)
            {
                maxBuyPrice += 2;
                buyOrderList.Add(new Order("Buy_OrderId" + i, maxBuyPrice, 20));
            }
            buyOrderList.Reverse();


            sellOrderList = new List<Order>();
            maxSellPrice = 0;
            for (int i = 0; i < ordersCount; i++)
            {
                maxBuyPrice += 2;
                sellOrderList.Add(new Order("Sell_OrderId" + i, maxBuyPrice, 20));
            }
        }

        [IterationSetup]
        public void Setup()
        {
            PrepareDataSet(OrderBookParam);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheBeginning), Benchmark]
        public void AddOrder_AtTheBeginning()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", maxBuyPrice + 2, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheEnd), Benchmark]
        public void AddOrder_AtTheEnd()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", 1, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheMiddle), Benchmark]
        public void AddOrder_InTheMiddle()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", (maxBuyPrice / 2) + 1, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheMiddleDuplicated), Benchmark]
        public void AddOrder_InTheMiddleDuplicated()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", maxBuyPrice / 2, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.RemoveAtTheBeginning), Benchmark]
        public void RemoveOrder_AtTheBeginning()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (ordersCount - 1));
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheEnd), Benchmark]
        public void RemoveOrder_AtTheEnd()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId0");
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheMiddle), Benchmark]
        public void RemoveOrder_InTheMiddle()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (ordersCount / 2));
        }

        [BenchmarkCategory(SingleOperationType.RemoveAtTheBeginning), Benchmark]
        public void RemoveSellOrder_AtTheBeginning()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId0");
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheEnd), Benchmark]
        public void RemoveSellOrder_AtTheEnd()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId" + (ordersCount - 1));
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheMiddle), Benchmark]
        public void RemoveSellOrder_InTheMiddle()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId" + (ordersCount / 2));
        }

        private void PrepareDataSet(IOrderBookTest orderBook)
        {
            orderBook.InidializeBuyOrders(buyOrderList);
            orderBook.InidializeSellOrders(sellOrderList);
        }
    }

    public static class SingleOperationType
    {
        public const string AddInTheBeginning = "AddInTheBeginning";
        public const string AddInTheMiddle = "AddInTheMiddle";
        public const string AddInTheMiddleDuplicated = "AddInTheMiddleDuplicated";
        public const string AddInTheEnd = "AddInTheEnd";
        public const string RemoveAtTheBeginning = "RemoveInTheBeginning";
        public const string RemoveInTheMiddle = "RemoveInTheMiddle";
        public const string RemoveInTheEnd = "RemoveInTheEnd";
    }
}
