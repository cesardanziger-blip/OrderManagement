using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
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

        public async Task CreateAsync(CreateProductRequest request)
        {
            var product = request.ToDomain();
            await _productRepository.CreateAsync(product);
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(c => c.ToResponse()).ToList();
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            var customer = await _productRepository.GetByIdAsync(id);

            return customer?.ToResponse();
        }

        public async Task UpdateAsync(UpdateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new Exception("Product not found.");

            product.UpdateDetails(request.Name, request.Description);
            product.UpdatePrice(request.Price);

            await _productRepository.UpdateAsync(product);
        }

        public async Task ActivateAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new Exception("Product not found.");

            product.Activate();

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeactivateAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new Exception("Product not found.");

            product.Deactivate();

            await _productRepository.UpdateAsync(product);
        }

        public async Task AddStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new Exception("Product not found.");

            product.IncreaseStock(quantity);

            await _productRepository.UpdateAsync(product);
        }

        public async Task RemoveStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new Exception("Product not found.");

            product.DecreaseStock(quantity);

            await _productRepository.UpdateAsync(product);
        }
    }
}
