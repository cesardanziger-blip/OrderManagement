using FluentAssertions;
using Moq;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Exceptions.Product;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.UnitTests.Application.Products
{
    public class ProductApplicationTests
    {
        private readonly Mock<IProductRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private ProductService CreateService()
            => new ProductService(_repo.Object, _uow.Object);


        //------> 1. CREATE PRODUCT
        [Fact]
        public async Task Should_Create_Product_Successfully()
        {
            var request = new CreateProductRequest
            {
                Name = "Mouse",
                Description = "Gaming",
                Price = 100,
                Stock = 10
            };

            var service = CreateService();

            var result = await service.CreateAsync(request);

            result.Should().NotBeNull();
            _repo.Verify(x => x.CreateAsync(It.IsAny<Product>()), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //------> 2. GET BY ID
        [Fact]
        public async Task Should_Return_Product_By_Id()
        {
            var product = new Product("Mouse", "desc", 10, 100);

            _repo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var service = CreateService();

            var result = await service.GetByIdAsync(product.Id);

            result.Id.Should().Be(product.Id);
        }

        //------> 3. NOT FOUND
        [Fact]
        public async Task Should_Throw_When_Product_Not_Found()
        {
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product?)null);

            var service = CreateService();

            Func<Task> act = async () =>
                await service.GetByIdAsync(Guid.NewGuid());

            await act.Should()
                .ThrowAsync<ProductNotFoundException>();
        }

        //------> 4. UPDATE PRODUCT (DETAILS + PRICE)
        [Fact]
        public async Task Should_Update_Product_Details_And_Price()
        {
            var product = new Product("Old", "desc", 10, 100);

            _repo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var service = CreateService();

            var request = new UpdateProductRequest
            {
                Name = "New Name",
                Description = "New Desc",
                Price = 200
            };

            await service.UpdateAsync(product.Id, request);

            product.Name.Should().Be("New Name");
            product.Description.Should().Be("New Desc");
            product.Price.Should().Be(200);

            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //------> 5. UPDATE PRICE INVALIDO (DOMAIN RULE)
        [Fact]
        public async Task Should_Throw_When_Update_Price_Is_Invalid()
        {
            var product = new Product("Mouse", "desc", 10, 100);

            _repo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var service = CreateService();

            var request = new UpdateProductRequest
            {
                Name = "Mouse",
                Description = "desc",
                Price = 0
            };

            Func<Task> act = async () =>
                await service.UpdateAsync(product.Id, request);

            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("*Price must be greater than zero*");
        }

        //------> 6. STATUS CHANGE
        [Fact]
        public async Task Should_Deactivate_Product()
        {
            var product = new Product("Mouse", "desc", 10, 100);

            _repo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var service = CreateService();

            await service.UpdateStatusAsync(product.Id,
                new UpdateProductStatusRequest { Active = false });

            product.Status.Should().Be(ProductStatus.Inactive);

            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        //------> 7. SET STOCK
        [Fact]
        public async Task Should_Set_Stock_Successfully()
        {
            var product = new Product("Mouse", "desc", 10, 100);

            _repo.Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            var service = CreateService();

            await service.SetStockAsync(product.Id,
                new SetProductStockRequest { Quantity = 50 });

            product.Stock.Should().Be(50);

            _uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
