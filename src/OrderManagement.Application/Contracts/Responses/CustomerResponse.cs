using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Contracts.Responses
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Document { get; set; } = String.Empty;
        public CustomerStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
