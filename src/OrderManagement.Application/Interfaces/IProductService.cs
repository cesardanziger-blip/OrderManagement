using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(CreateProductRequest request);
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();

        Task UpdateAsync(UpdateProductRequest request);

        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);

        Task AddStockAsync(Guid productId, int quantity);
        Task RemoveStockAsync(Guid productId, int quantity);
    }
}
