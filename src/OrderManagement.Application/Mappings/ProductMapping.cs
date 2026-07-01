using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Mappings
{
    public static class ProductMapping
    {
        public static ProductResponse ToResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Status = product.Status,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public static Product ToDomain(this CreateProductRequest request)
        {
            return new Product(request.Name, request.Description, request.Stock, request.Price);
        }
    }
}