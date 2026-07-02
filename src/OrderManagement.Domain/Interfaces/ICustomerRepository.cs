using OrderManagement.Domain.Entities;

namespace OrderManagement.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task CreateAsync(Customer customer);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentExistsAsync(string document);
    }
}
