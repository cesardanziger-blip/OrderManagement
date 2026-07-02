using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Product
{
    public class InsufficientStockException : BusinessException
    {
        public InsufficientStockException(Guid productId, string productName)
            : base($"Insufficient stock for product '{productName}' ({productId}).")
        {
        }
    }
}
