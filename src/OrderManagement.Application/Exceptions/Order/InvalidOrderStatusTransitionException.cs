using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Order
{
    public class InvalidOrderStatusTransitionException : BusinessException
    {
        public InvalidOrderStatusTransitionException()
            : base("Invalid order status transition.")
        {
        }
    }
}
