namespace OrderManagement.Application.Contracts.Requests
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public decimal Price { get; set; }
    }
}