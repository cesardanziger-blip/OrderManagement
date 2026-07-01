using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;

namespace OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(CreateProductRequest request);
        Task<ProductResponse?> GetByIdAsync(Guid id);
        Task<List<ProductResponse>> GetAllAsync();

        Task UpdateAsync(UpdateProductRequest request);

        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);

        Task AddStockAsync(Guid productId, int quantity);
        Task RemoveStockAsync(Guid productId, int quantity);
    }
}
