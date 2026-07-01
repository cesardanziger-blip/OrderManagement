using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities
{
    public class OrderHistory
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public OrderStatus PreviousStatus { get; private set; }
        public OrderStatus NewStatus { get; private set; }
        public DateTime ModificationDate { get; private set; }
        public string? Reason { get; private set; } = string.Empty;

        private OrderHistory() { }

        public OrderHistory(Guid orderId, OrderStatus previousStatus, OrderStatus newStatus, string? reason)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            ModificationDate = DateTime.UtcNow;
            Reason = reason ?? string.Empty;
        }
    }
}
