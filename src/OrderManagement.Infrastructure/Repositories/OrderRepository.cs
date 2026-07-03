using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Infrastructure.Context;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderManagementDbContext _context;

        public OrderRepository(OrderManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(x => x.Items)
                            .Include(x => x.History)
                            .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders.Include(x => x.Items)
                                        .Include(x => x.History)
                                        .FirstOrDefaultAsync(x => x.Id == id);            
        }
    }
}
