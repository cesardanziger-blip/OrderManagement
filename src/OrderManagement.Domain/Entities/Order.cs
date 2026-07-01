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
        private readonly List<OrderHistory> _history = [];

        public IReadOnlyCollection<OrderHistory> History => _history.AsReadOnly();

        private Order() { }
        public Order(Guid customerId, string? reason = null)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Created;
            _history.Add(new OrderHistory(Id, OrderStatus.Created, OrderStatus.Created, reason));
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
        private void SetStatus(OrderStatus newStatus, string? reason)
        {
            var previousStatus = Status;
            Status = newStatus;
            _history.Add(new OrderHistory(Id, previousStatus, newStatus, reason));
        }

        public void MarkAsPaid(string? reason = null)
        {
            if (Status != OrderStatus.Created)
                throw new InvalidOperationException("Only created orders can be paid.");

            SetStatus(OrderStatus.Paid, reason);
        }
        public void MarkAsShipped(string? reason = null)
        {
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only paid orders can be shipped.");

            SetStatus(OrderStatus.Shipped, reason);
        }
        public void MarkAsCancelled(string? reason = null)
        {
            if (Status != OrderStatus.Created)
                throw new InvalidOperationException("Only created orders can be cancelled.");

            SetStatus(OrderStatus.Cancelled, reason);
        }
    }
}
