using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.Application.Exceptions.Customer
{
    public class InactiveCustomerException : BusinessException
    {
        public InactiveCustomerException(Guid id)
            : base($"Customer '{id}' is inactive and cannot create orders.")
        {
        }
    }
}
