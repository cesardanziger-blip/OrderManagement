using OrderManagement.Application.Contracts.Requests;

namespace OrderManagement.Application.Validators.Order
{
    public class CreateOrderCommand
    {
        public Guid CustomerId { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = [];
    }
}
