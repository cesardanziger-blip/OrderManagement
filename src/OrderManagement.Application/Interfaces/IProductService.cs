using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse?> GetByIdAsync(Guid id);
        Task<List<ProductResponse>> GetAllAsync();

        Task UpdateAsync(UpdateProductRequest request);
        Task UpdateStatusAsync(UpdateProductStatusRequest request);
        Task UpdateStockAsync(UpdateProductStockRequest request);
    }
}
