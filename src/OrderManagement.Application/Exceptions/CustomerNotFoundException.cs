namespace OrderManagement.Application.Exceptions
{
    public sealed class CustomerNotFoundException : NotFoundException
    {
        public CustomerNotFoundException(Guid id)
            : base($"Customer with id '{id}' was not found.")
        {
        }
    }
}
