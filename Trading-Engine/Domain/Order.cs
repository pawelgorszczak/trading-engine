namespace Trading_Engine.Domain
{
    public class Order
    {

        public string OrderId { get; set; }
        public int Price { get; set; }
        public virtual int Quantity { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public Order(string orderId, int price, int quantity)
        {
            OrderId = orderId;
            Price = price;
            Quantity = quantity;
            CreateTime = DateTimeOffset.Now;
        }

        public override string ToString() => $"{Quantity}@{Price}#{OrderId}";
    }
}