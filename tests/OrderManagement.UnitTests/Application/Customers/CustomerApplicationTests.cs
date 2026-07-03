using FluentAssertions;
using Moq;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Exceptions.Customer;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.UnitTests.Application
{
    public class CustomerApplicationTests
    {
        private readonly Mock<ICustomerRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private CustomerService CreateService()
            => new CustomerService(_repo.Object, _uow.Object);

        //----> 1. CREATE CUSTOMER - sucesso
        [Fact]
        public async Task Should_Create_Customer_Successfully()
        {
            // Arrange
            var request = new CreateCustomerRequest
            {
                Name = "John",
                Email = "john@test.com",
                Document = "123"
            };

            _repo.Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(false);

            _repo.Setup(x => x.DocumentExistsAsync(request.Document))
                .ReturnsAsync(false);

            var service = CreateService();

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            _repo.Verify(x => x.CreateAsync(It.IsAny<Customer>()), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //----> 2.EMAIL DUPLICADO
        [Fact]
        public async Task Should_Throw_When_Email_Already_Exists()
        {
            var request = new CreateCustomerRequest
            {
                Name = "John",
                Email = "duplicate@test.com",
                Document = "123"
            };

            _repo.Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(true);

            var service = CreateService();

            Func<Task> act = async () => await service.CreateAsync(request);

            await act.Should()
                .ThrowAsync<DuplicateCustomerException>()
                .WithMessage("*email*");
        }

        //----> 3.DOCUMENT DUPLICADO
        [Fact]
        public async Task Should_Throw_When_Document_Already_Exists()
        {
            var request = new CreateCustomerRequest
            {
                Name = "John",
                Email = "test@test.com",
                Document = "999"
            };

            _repo.Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(false);

            _repo.Setup(x => x.DocumentExistsAsync(request.Document))
                .ReturnsAsync(true);

            var service = CreateService();

            Func<Task> act = async () => await service.CreateAsync(request);

            await act.Should()
                .ThrowAsync<DuplicateCustomerException>();
        }

        //----> 4.GET BY ID
        [Fact]
        public async Task Should_Return_Customer_By_Id()
        {
            var customer = new Customer("John", "john@test.com", "123");

            _repo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            var service = CreateService();

            var result = await service.GetByIdAsync(customer.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(customer.Id);
        }

        //----> 4. CUSTOMER NÃO ENCONTRADO
        [Fact]
        public async Task Should_Throw_When_Customer_Not_Found()
        {
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Customer?)null);

            var service = CreateService();

            Func<Task> act = async () =>
                await service.GetByIdAsync(Guid.NewGuid());

            await act.Should()
                .ThrowAsync<CustomerNotFoundException>();
        }

        //----> 5.ATIVAR CUSTOMER
        [Fact]
        public async Task Should_Activate_Customer_Successfully()
        {
            var customer = new Customer("John", "john@test.com", "123");
            customer.Deactivate();

            _repo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            var service = CreateService();

            await service.UpdateStatusAsync(customer.Id,
                new UpdateCustomerStatusRequest { Active = true });

            customer.Status.Should().Be(CustomerStatus.Active);

            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //----> 6.DESATIVAR CUSTOMER
        [Fact]
        public async Task Should_Deactivate_Customer_Successfully()
        {
            var customer = new Customer("John", "john@test.com", "123");

            _repo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            var service = CreateService();

            await service.UpdateStatusAsync(customer.Id,
                new UpdateCustomerStatusRequest { Active = false });

            customer.Status.Should().Be(CustomerStatus.Inactive);

            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //----> 7. NÃO DESATIVAR JÁ INATIVO
        [Fact]
        public async Task Should_Throw_When_Deactivating_Already_Inactive()
        {
            var customer = new Customer("John", "john@test.com", "123");
            customer.Deactivate();

            _repo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            var service = CreateService();

            Func<Task> act = async () =>
                await service.UpdateStatusAsync(customer.Id,
                    new UpdateCustomerStatusRequest { Active = false });

            await act.Should()
                .ThrowAsync<InvalidOperationException>();
        }

        //----> 8. NÃO ATIVAR JÁ ATIVO
        [Fact]
        public async Task Should_Throw_When_Activating_Already_Active()
        {
            var customer = new Customer("John", "john@test.com", "123");

            _repo.Setup(x => x.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            var service = CreateService();

            Func<Task> act = async () =>
                await service.UpdateStatusAsync(customer.Id,
                    new UpdateCustomerStatusRequest { Active = true });

            await act.Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}
