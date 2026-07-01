namespace OrderManagement.Application.Contracts.Requests
{
    public class CreateCustomerRequest
    {
        public string Name { get; private set; } = String.Empty;
        public string Email { get; private set; } = String.Empty;
        public string Document { get; private set; } = String.Empty;
    }
}
