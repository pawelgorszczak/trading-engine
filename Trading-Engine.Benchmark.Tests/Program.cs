using BenchmarkDotNet.Running;
using Trading_Engine.Benchmark.Tests.OrderBook;

//new OrderBookTests().AddBuyAndSellOrder3();

BenchmarkRunner.Run<OrderBookTests>();