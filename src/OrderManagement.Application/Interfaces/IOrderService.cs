using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse> GetByIdAsync(Guid id);
        Task<PagedResponse<OrderResponse>> GetAllAsync(PagedRequest request);
        Task UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request);
    }
}
