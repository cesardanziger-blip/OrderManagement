using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Product
{
    public class InactiveProductException : BusinessException
    {
        public InactiveProductException(Guid id)
            : base($"Product '{id}' is inactive and cannot be used in orders.")
        {
        }
    }
}
