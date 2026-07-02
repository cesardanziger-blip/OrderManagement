using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = String.Empty;
        public string Description { get; private set; } = String.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public ProductStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Product() { }

        public Product(string name, string description, int stock, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (stock < 0)
                throw new ArgumentException("Quantity cant be negative.", nameof(stock));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            Id = Guid.NewGuid();
            Name = name;
            Description = description ?? String.Empty;
            Stock = stock;
            Price = price;
            Status = ProductStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if (Status == ProductStatus.Inactive)
                throw new InvalidOperationException("Product is already inactive.");

            Status = ProductStatus.Inactive;
            Touch();
        }
        public void Activate()
        {
            if (Status == ProductStatus.Active)
                throw new InvalidOperationException("Product is already active.");

            Status = ProductStatus.Active;
            Touch();
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity cant be negative.", nameof(quantity));

            if (Stock < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            Stock -= quantity;
            Touch();
        }

        public void IncreaseStock(int quantity)
        {
            if (Status != ProductStatus.Active)
                throw new InvalidOperationException("Inactive product cannot have its stock changed.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity cant be negative.", nameof(quantity));

            Stock += quantity;
            Touch();
        }

        public void SetStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Stock cannot be negative.", nameof(quantity));

            if (quantity == Stock)
                return;

            Stock = quantity;
            Touch();
        }

        public void UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            Name = name;
            Description = description ?? String.Empty;
            Touch();
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));

            Price = newPrice;
            Touch();
        }
        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
