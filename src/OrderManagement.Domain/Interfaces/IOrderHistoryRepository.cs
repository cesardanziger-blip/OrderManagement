using OrderManagement.Domain.Entities;

namespace OrderManagement.Domain.Interfaces
{
    public interface IOrderHistoryRepository
    {
        Task AddAsync(OrderHistory history);
    }
}
