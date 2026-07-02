using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse> GetByIdAsync(Guid id);
        Task<List<OrderResponse>> GetAllAsync();
        Task UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request);
    }
}
