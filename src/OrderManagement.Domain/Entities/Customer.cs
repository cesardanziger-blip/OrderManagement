using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = String.Empty;
        public string Email { get; private set; } = String.Empty;
        public string Document { get; private set; } = String.Empty;
        public CustomerStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Customer() { }

        public Customer(string name, string email, string document)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(document))
                throw new ArgumentException("Document cannot be empty.", nameof(document));

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Document = document;
            Status = CustomerStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            Status = CustomerStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Activate()
        {
            Status = CustomerStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
