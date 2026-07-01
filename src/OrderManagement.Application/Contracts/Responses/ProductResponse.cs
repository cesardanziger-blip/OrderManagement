using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Contracts.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
