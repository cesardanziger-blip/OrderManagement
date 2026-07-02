namespace OrderManagement.Application.Contracts.Requests
{
    public class UpdateProductStockRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}