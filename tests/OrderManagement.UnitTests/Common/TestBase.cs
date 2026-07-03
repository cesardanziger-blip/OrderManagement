using Moq;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.UnitTests.Common
{
    public abstract class TestBase
    {
        protected readonly Mock<IOrderRepository> OrderRepositoryMock;
        protected readonly Mock<IOrderHistoryRepository> OrderHistoryRepositoryMock;
        protected readonly Mock<IProductRepository> ProductRepositoryMock;
        protected readonly Mock<ICustomerRepository> CustomerRepositoryMock;
        protected readonly Mock<IUnitOfWork> UnitOfWorkMock;

        protected readonly OrderService OrderService;

        protected TestBase()
        {
            OrderRepositoryMock = new Mock<IOrderRepository>();
            OrderHistoryRepositoryMock = new Mock<IOrderHistoryRepository>();
            ProductRepositoryMock = new Mock<IProductRepository>();
            CustomerRepositoryMock = new Mock<ICustomerRepository>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();

            OrderService = new OrderService(
                OrderRepositoryMock.Object,
                ProductRepositoryMock.Object,
                CustomerRepositoryMock.Object,
                OrderHistoryRepositoryMock.Object,
                UnitOfWorkMock.Object
            );
        }
    }
}
