using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Infrastructure.Context;

namespace OrderManagement.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OrderManagementDbContext _context;

        public CustomerRepository(OrderManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _context.Customers.AnyAsync(x =>x.Email == email && x.Status == CustomerStatus.Active);
        }

        public Task<bool> DocumentExistsAsync(string document)
        {
            return _context.Customers.AnyAsync(x => x.Document == document && x.Status == CustomerStatus.Active);
        }
    }
}
