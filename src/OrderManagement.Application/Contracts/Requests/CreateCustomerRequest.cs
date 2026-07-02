namespace OrderManagement.Application.Contracts.Requests
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Document { get; set; } = String.Empty;
    }
}
