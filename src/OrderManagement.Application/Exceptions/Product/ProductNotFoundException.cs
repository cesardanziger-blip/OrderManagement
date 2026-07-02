using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Product
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid id)
            : base($"Product with id '{id}' was not found.")
        {
        }
    }
}
