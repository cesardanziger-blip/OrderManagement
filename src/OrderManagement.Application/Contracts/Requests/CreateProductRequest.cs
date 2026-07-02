namespace OrderManagement.Application.Contracts.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
