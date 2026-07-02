using OrderManagement.Application.Exceptions.Common;

public class DuplicateCustomerException : BusinessException
{
    public DuplicateCustomerException(string field)
        : base($"A customer with this {field} already exists.")
    {
    }
}