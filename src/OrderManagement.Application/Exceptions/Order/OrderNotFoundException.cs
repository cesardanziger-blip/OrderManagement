using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Order
{
    public sealed class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id)
            : base($"Order with id '{id}' was not found.")
        {
        }
    }
}
