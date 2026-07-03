using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Mappings
{
    public static class CustomerMapping
    {
        public static CustomerResponse ToResponse(this Customer customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Document = customer.Document,
                Status = customer.Status,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }

        public static Customer ToDomain(this CreateCustomerRequest request)
        {
            return new Customer(request.Name, request.Email, request.Document);
        }
    }
}