using FluentAssertions;
using Moq;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Exceptions.Product;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.UnitTests.Application.Orders
{
    public class OrderStockConsistencyTests
    {
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<ICustomerRepository> _customerRepo = new();
        private readonly Mock<IOrderHistoryRepository> _historyRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly List<Order> _orders = new();

        private OrderService CreateService()
            => new OrderService(
                _orderRepo.Object,
                _productRepo.Object,
                _customerRepo.Object,
                _historyRepo.Object,
                _uow.Object
            );

        [Fact]
        public async Task Should_Not_Decrease_Any_Stock_When_One_Item_Fails()
        {
            var customer = new Customer("John", "john@test.com", "123");

            var product1 = new Product("P1", "desc", 10, 100);
            var product2 = new Product("P2", "desc", 1, 100); // vai falhar

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _productRepo.Setup(x => x.GetByIdAsync(product1.Id))
                .ReturnsAsync(product1);

            _productRepo.Setup(x => x.GetByIdAsync(product2.Id))
                .ReturnsAsync(product2);

            var initialStock1 = product1.Stock;

            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>
                {
                    new() { ProductId = product1.Id, Quantity = 2 },
                    new() { ProductId = product2.Id, Quantity = 5 } // insuficiente
                }
            };

            var service = CreateService();

            Func<Task> act = async () => await service.CreateAsync(request);

            await act.Should()
                .ThrowAsync<InsufficientStockException>();

            product1.Stock.Should().Be(initialStock1);
        }

        // ----> Order Cancell - stock test
        [Fact]
        public async Task Should_Return_Stock_When_Order_Is_Cancelled()
        {
            var customer = new Customer("John", "john@test.com", "123");
            var product = new Product("P1", "desc", 10, 100);

            _orderRepo.Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .Callback<Order>(o => _orders.Add(o));

            _orderRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => _orders.FirstOrDefault(x => x.Id == id));

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _productRepo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);


            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>
                {
                    new() { ProductId = product.Id, Quantity = 3 }
                }
            };

            var service = CreateService();

            var order = await service.CreateAsync(request);

            // simula cancelamento
            var update = new UpdateOrderStatusRequest
            {
                Status = OrderStatus.Cancelled,
                Reason = "test"
            };

            await service.UpdateStatusAsync(order.Id, update);

            product.Stock.Should().Be(10); // voltou ao original
        }
    }
}