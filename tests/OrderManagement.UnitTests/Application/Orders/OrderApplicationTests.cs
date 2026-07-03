using FluentAssertions;
using Moq;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Exceptions.Customer;
using OrderManagement.Application.Exceptions.Product;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.UnitTests.Application.Orders
{
    public class OrderApplicationTests
    {
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<ICustomerRepository> _customerRepo = new();
        private readonly Mock<IOrderHistoryRepository> _historyRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private OrderService CreateService()
            => new OrderService(
                _orderRepo.Object,
                _productRepo.Object,
                _customerRepo.Object,
                _historyRepo.Object,
                _uow.Object
            );

        //--------> 1. CREATE ORDER SUCCESS
        [Fact]
        public async Task Should_Create_Order_Successfully()
        {
            var customer = new Customer("John", "john@test.com", "123");

            var product = new Product("Mouse", "desc", 10, 100);

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _productRepo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>
                {
                    new() { ProductId = product.Id, Quantity = 2 }
                }
            };

            var service = CreateService();

            var result = await service.CreateAsync(request);

            result.Should().NotBeNull();

            _orderRepo.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //--------> 1. CREATE ORDER - MULTI ITEM SUCCESS
        [Fact]
        public async Task Should_Calculate_Total_For_Multiple_Items()
        {
            var customer = new Customer("John", "john@test.com", "123");

            var product1 = new Product("P1", "desc", 10, 50);
            var product2 = new Product("P2", "desc", 10, 100);

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _productRepo.Setup(x => x.GetByIdAsync(product1.Id))
                .ReturnsAsync(product1);

            _productRepo.Setup(x => x.GetByIdAsync(product2.Id))
                .ReturnsAsync(product2);

            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>
                {
                    new() { ProductId = product1.Id, Quantity = 2 }, // 100
                    new() { ProductId = product2.Id, Quantity = 1 }  // 100
                }
            };

            var service = CreateService();

            var result = await service.CreateAsync(request);

            result.Total.Should().Be(200);
        }

        //--------> 2. CLIENTE INATIVO
        [Fact]
        public async Task Should_Throw_When_Customer_Is_Inactive()
        {
            var customer = new Customer("John", "john@test.com", "123");
            customer.Deactivate();

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            var service = CreateService();

            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>()
            };

            Func<Task> act = async () =>
                await service.CreateAsync(request);

            await act.Should()
                .ThrowAsync<InactiveCustomerException>();
        }

        //--------> 3. PRODUTO INATIVO
        [Fact]
        public async Task Should_Throw_When_Product_Is_Inactive()
        {
            var customer = new Customer("John", "john@test.com", "123");

            var product = new Product("Mouse", "desc", 10, 100);
            product.Deactivate();

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _productRepo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>
                {
                    new() { ProductId = product.Id, Quantity = 1 }
                }
            };

            var service = CreateService();

            Func<Task> act = async () =>
                await service.CreateAsync(request);

            await act.Should()
                .ThrowAsync<InactiveProductException>();
        }

        //--------> 4. ESTOQUE INSUFICIENTE
        [Fact]
        public async Task Should_Throw_When_Stock_Is_Insufficient()
        {
            var customer = new Customer("John", "john@test.com", "123");

            var product = new Product("Mouse", "desc", 1, 100);

            _customerRepo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _productRepo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var request = new CreateOrderRequest
            {
                CustomerId = customer.Id,
                Items = new List<CreateOrderItemRequest>
                {
                    new() { ProductId = product.Id, Quantity = 5 }
                }
            };

            var service = CreateService();

            Func<Task> act = async () =>
                await service.CreateAsync(request);

            await act.Should()
                .ThrowAsync<InsufficientStockException>();
        }
    }
}
