namespace OrderManagement.Application.Contracts.Requests
{
    public class UpdateProductStatusRequest
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
    }
}