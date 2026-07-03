using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Contracts.Requests
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
