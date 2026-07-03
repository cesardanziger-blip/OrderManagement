using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse> GetByIdAsync(Guid id);
        Task<PagedResponse<ProductResponse>> GetAllAsync(PagedRequest request);

        Task UpdateAsync(Guid id, UpdateProductRequest request);
        Task UpdateStatusAsync(Guid id, UpdateProductStatusRequest request);
        Task SetStockAsync(Guid id, SetProductStockRequest request);
    }
}
