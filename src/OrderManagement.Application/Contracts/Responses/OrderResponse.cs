namespace OrderManagement.Application.Contracts.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<OrderItemResponse> Items { get; set; } = [];
        public List<OrderHistoryResponse> History { get; set; } = [];
    }
}
