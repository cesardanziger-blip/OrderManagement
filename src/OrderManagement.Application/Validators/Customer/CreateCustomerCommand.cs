namespace OrderManagement.Application.Validators.Customer
{
    public class CreateCustomerCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
    }
}
