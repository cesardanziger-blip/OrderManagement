using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Contracts.Requests
{
    public class UpdateOrderStatusRequest
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
