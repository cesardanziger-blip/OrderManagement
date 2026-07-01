namespace OrderManagement.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Total { get; private set; }
        private OrderItem() { }

        public OrderItem(Guid orderId, Product product, int quantity)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductId = product.Id;
            Quantity = quantity;
            UnitPrice = product.Price;
            Total = UnitPrice * quantity;
        }
    }
}
