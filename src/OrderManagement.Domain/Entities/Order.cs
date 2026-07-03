using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal Total { get; private set; }
        public OrderStatus Status { get; private set; }
        private readonly List<OrderItem> _items = [];
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        private readonly List<OrderHistory> _history = new();

        public IReadOnlyCollection<OrderHistory> History => _history.AsReadOnly();

        private Order() { }
        public Order(Guid customerId, string? reason = null)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Created;
        }
        public void AddItem(Product product, int quantity)
        {
            if (Status != OrderStatus.Created)
                throw new InvalidOperationException("Items can only be added to a newly created order.");

            var item = new OrderItem(Id, product, quantity);

            _items.Add(item);

            RecalculateTotal();
        }
        private void RecalculateTotal()
        {
            Total = _items.Sum(i => i.Total);
        }

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Created)
                throw new InvalidOperationException();

            Status = OrderStatus.Paid;
        }

        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only paid orders can be shipped.");

            Status = OrderStatus.Shipped;
        }

        public void MarkAsCancelled(string? reason = null)
        {
            if (Status == OrderStatus.Shipped)
                throw new InvalidOperationException("Shipped order cannot be cancelled.");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order already cancelled.");

            var previousStatus = Status;
            Status = OrderStatus.Cancelled;
        }
    }
}
