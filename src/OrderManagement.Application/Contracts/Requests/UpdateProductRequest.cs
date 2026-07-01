namespace OrderManagement.Application.Contracts.Requests
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; private set; } = String.Empty;
        public string Description { get; private set; } = String.Empty;
        public decimal Price { get; private set; }
    }
}