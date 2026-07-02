using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Customer
{
    public sealed class CustomerNotFoundException : NotFoundException
    {
        public CustomerNotFoundException(Guid id)
            : base($"Customer with id '{id}' was not found.")
        {
        }
    }
}
