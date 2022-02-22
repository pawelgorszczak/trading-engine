namespace Trading_Engine.Domain
{
    public class IceBeargOrder : Order
    {
        public override int Quantity { get; set; }
        public int TotalQuantity { get; set; }
        public int DisplaySize { get; set; }

        public IceBeargOrder(string orderId, int price, int quantity) : base(orderId, price, quantity)
        {
        }

        public override string ToString() => $"{Quantity}({TotalQuantity})@{Price}#{OrderId}";
    }
}