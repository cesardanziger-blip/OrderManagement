using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateAsync(CreateOrderRequest request);
        Task<Order?> GetByIdAsync(Guid id);
        Task<List<Order>> GetAllAsync();

        Task PayAsync(Guid id);
        Task ShipAsync(Guid id);
        Task CancelAsync(Guid id);
    }
}
