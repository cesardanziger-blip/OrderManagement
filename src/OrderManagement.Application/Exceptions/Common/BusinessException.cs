namespace OrderManagement.Application.Exceptions.Common;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}