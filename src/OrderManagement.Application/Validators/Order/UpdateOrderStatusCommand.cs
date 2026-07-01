using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Validators.Order
{
    public class UpdateOrderStatusCommand
    {
        public Guid OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string? Reason { get; set; }
    }
}
