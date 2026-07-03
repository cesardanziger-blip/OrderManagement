using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Infrastructure.Context;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly OrderManagementDbContext _context;

        public OrderHistoryRepository(OrderManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderHistory history)
        {
            await _context.OrderHistories.AddAsync(history);
        }
    }
}
