namespace OrderManagement.Application.Contracts.Responses
{
    public class OrderHistoryResponse
    {
        public Guid Id { get; set; }
        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public string? Reason { get; set; }
    }
}
