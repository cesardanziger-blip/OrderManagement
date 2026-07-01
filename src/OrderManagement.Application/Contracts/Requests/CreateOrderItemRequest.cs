namespace OrderManagement.Application.Contracts.Requests
{
    public class CreateOrderItemRequest
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
    }
}
