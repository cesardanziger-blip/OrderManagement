using FluentAssertions;
using Moq;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.UnitTests.Application.Orders
{
    public class OrderHistoryTests
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

        [Fact]
        public async Task Should_Create_Order_History_On_Creation()
        {
            var customer = new Customer("John", "john@test.com", "123");
            var product = new Product("P1", "desc", 10, 100);

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

            await service.CreateAsync(request);

            _historyRepo.Verify(x =>
                x.AddAsync(It.IsAny<OrderHistory>()),
                Times.Once);
        }
    }
}