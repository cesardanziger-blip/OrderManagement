using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Exceptions.Customer;
using OrderManagement.Application.Exceptions.Order;
using OrderManagement.Application.Exceptions.Product;
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
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(CreateOrderRequest request)
        {
            var customerId = request.CustomerId;

            var customer = await _customerRepository.GetByIdAsync(customerId)
                ?? throw new CustomerNotFoundException(customerId);

            if (customer.Status != CustomerStatus.Active)
                throw new InactiveCustomerException(customerId);

            var order = new Order(customerId);

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ProductNotFoundException(item.ProductId);

                if (product.Status != ProductStatus.Active)
                    throw new InactiveProductException(item.ProductId);

                if (product.Stock < item.Quantity)
                    throw new InsufficientStockException(product.Id,product.Name);

                product.DecreaseStock(item.Quantity);

                order.AddItem(product, item.Quantity);
            }

            await _orderRepository.CreateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order.Id;
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order.ToResponse();
        }

        public async Task<List<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(c => c.ToResponse()).ToList();
        }

        public async Task UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new OrderNotFoundException(id);

            switch (request.Status)
            {
                case OrderStatus.Paid:
                    order.MarkAsPaid(request.Reason);
                    break;

                case OrderStatus.Shipped:
                    order.MarkAsShipped(request.Reason);
                    break;

                case OrderStatus.Cancelled:
                    order.MarkAsCancelled(request.Reason);

                    foreach (var item in order.Items)
                    {
                        var product = await _productRepository.GetByIdAsync(item.ProductId)
                             ?? throw new ProductNotFoundException(item.ProductId);

                        product.IncreaseStock(item.Quantity);
                    }

                    break;

                default:
                    throw new InvalidOrderStatusTransitionException();
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}