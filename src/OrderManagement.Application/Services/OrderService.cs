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
        private readonly IOrderHistoryRepository _orderHistoryRepository;

        public OrderService(IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IOrderHistoryRepository orderHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw new CustomerNotFoundException(request.CustomerId);

            if (customer.Status != CustomerStatus.Active)
                throw new InactiveCustomerException(request.CustomerId);

            var order = new Order(customer.Id);

            var products = new Dictionary<Guid, Product>();

            //Validar
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ProductNotFoundException(item.ProductId);

                if (product.Status != ProductStatus.Active)
                    throw new InactiveProductException(item.ProductId);

                if (product.Stock < item.Quantity)
                    throw new InsufficientStockException(product.Id, product.Name);

                products[item.ProductId] = product;
            }

            //Aplicar
            foreach (var item in request.Items)
            {
                var product = products[item.ProductId];

                product.DecreaseStock(item.Quantity);
                order.AddItem(product, item.Quantity);
            }
            await _orderRepository.CreateAsync(order);

            await _orderHistoryRepository.AddAsync(
                OrderHistory.Create(order)
            );

            await _unitOfWork.SaveChangesAsync();

            return order.ToResponse();
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new OrderNotFoundException(id);

            return order.ToResponse();
        }

        public async Task<PagedResponse<OrderResponse>> GetAllAsync(PagedRequest request)
        {
            var query = await _orderRepository.GetAllAsync();

            var totalCount = query.Count();

            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => o.ToResponse())
                .ToList();

            return new PagedResponse<OrderResponse>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new OrderNotFoundException(id);

            var previousStatus = order.Status;

            if (request.Status == order.Status)
                throw new InvalidOrderStatusTransitionException();

            switch (request.Status)
            {
                case OrderStatus.Paid:
                    order.MarkAsPaid();
                    break;

                case OrderStatus.Shipped:
                    order.MarkAsShipped();
                    break;

                case OrderStatus.Cancelled:
                    {
                        if (previousStatus == OrderStatus.Shipped)
                            throw new InvalidOperationException("Shipped orders cannot be cancelled.");

                        order.MarkAsCancelled(request.Reason);

                        foreach (var item in order.Items)
                        {
                            var product = await _productRepository.GetByIdAsync(item.ProductId)
                                ?? throw new ProductNotFoundException(item.ProductId);

                            product.IncreaseStock(item.Quantity);
                        }

                        break;
                    }
            }

            await _orderHistoryRepository.AddAsync(
                new OrderHistory(order.Id, previousStatus, order.Status, request.Reason)
            );

            await _unitOfWork.SaveChangesAsync();
        }
    }
}