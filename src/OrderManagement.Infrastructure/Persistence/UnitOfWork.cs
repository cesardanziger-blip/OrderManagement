using OrderManagement.Application.Interfaces;
using OrderManagement.Infrastructure.Context;

namespace OrderManagement.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderManagementDbContext _context;

        public UnitOfWork(OrderManagementDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
