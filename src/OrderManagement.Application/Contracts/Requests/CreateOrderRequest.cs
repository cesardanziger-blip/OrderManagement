namespace OrderManagement.Application.Contracts.Requests
{
    public class CreateOrderRequest
    {
        public Guid CustomerId { get; private set; }
        public List<CreateOrderItemRequest> Items { get; set; } = [];
    }
}
