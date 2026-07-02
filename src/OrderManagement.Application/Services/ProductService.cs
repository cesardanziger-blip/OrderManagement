using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Exceptions.Product;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Mappings;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var product = request.ToDomain();
            await _productRepository.CreateAsync(product);

            return product.ToResponse();
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(c => c.ToResponse()).ToList();
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new ProductNotFoundException(id);

            return product?.ToResponse();
        }

        public async Task UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new ProductNotFoundException(id);

            product.UpdateDetails(request.Name, request.Description);
            product.UpdatePrice(request.Price);

            await _productRepository.UpdateAsync(product);
        }

        public async Task UpdateStatusAsync(Guid id, UpdateProductStatusRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id)
                 ?? throw new ProductNotFoundException(id);

            if (request.Active)
                product.Activate();
            else
                product.Deactivate();

            await _productRepository.UpdateAsync(product);
        }

        public async Task SetStockAsync(Guid id, SetProductStockRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id)
                 ?? throw new ProductNotFoundException(id);

            product.SetStock(request.Quantity);

            await _productRepository.UpdateAsync(product);
        }
    }
}
