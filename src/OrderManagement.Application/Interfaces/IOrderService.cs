using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<List<OrderResponse>> GetAllAsync();

        Task PayAsync(Guid id);
        Task ShipAsync(Guid id);
        Task CancelAsync(Guid id);
    }
}
