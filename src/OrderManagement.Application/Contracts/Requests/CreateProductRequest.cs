namespace OrderManagement.Application.Contracts.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; private set; } = String.Empty;
        public string Description { get; private set; } = String.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
    }
}
