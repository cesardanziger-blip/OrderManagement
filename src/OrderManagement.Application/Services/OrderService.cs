using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Mappings;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderService(IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Guid> CreateAsync(CreateOrderRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw new Exception("Customer not found.");

            if (customer.Status != CustomerStatus.Active)
                throw new Exception("Inactive customer cannot create orders.");

            var order =  new Order(request.CustomerId);

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new Exception("Product not found.");

                if (product.Status != ProductStatus.Active)
                    throw new Exception("Inactive product cannot be used.");

                product.DecreaseStock(item.Quantity);

                order.AddItem(product, item.Quantity);

                await _productRepository.UpdateAsync(product);
            }

            await _orderRepository.CreateAsync(order);

            return order.Id;
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order?.ToResponse();
        }

        public async Task<List<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(c => c.ToResponse()).ToList();
        }

        public async Task PayAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new Exception("Order not found.");

            order.MarkAsPaid();

            await _orderRepository.UpdateAsync(order);
        }

        public async Task ShipAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new Exception("Order not found.");

            order.MarkAsShipped();

            await _orderRepository.UpdateAsync(order);
        }

        public async Task CancelAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new Exception("Order not found.");

            order.MarkAsCancelled();

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                product?.IncreaseStock(item.Quantity);
                await _productRepository.UpdateAsync(product!);
            }

            await _orderRepository.UpdateAsync(order);
        }
    }
}