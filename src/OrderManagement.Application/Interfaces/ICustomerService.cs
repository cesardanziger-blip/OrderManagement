using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface ICustomerService
    {
        Task CreateAsync(CreateCustomerRequest customer);
        Task<List<CustomerResponse>> GetAllAsync();
        Task<CustomerResponse?> GetByIdAsync(Guid id);

        Task DeactivateAsync(Guid id);
    }
}