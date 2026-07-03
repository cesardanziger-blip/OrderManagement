using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.UnitTests.Application.Products
{
    public class ProductValidationTests
    {
        // 1. Nome vazio
        [Fact]
        public void Should_Throw_When_Name_Is_Empty()
        {
            Action act = () =>
                new Product("", "desc", 10, 10);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Name cannot be empty*");
        }

        // 2. Estoque negativo
        [Fact]
        public void Should_Throw_When_Stock_Is_Negative()
        {
            Action act = () =>
                new Product("Product", "desc", -1, 10);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Quantity cant be negative*");
        }

        // 3. Preço inválido
        [Fact]
        public void Should_Throw_When_Price_Is_Invalid()
        {
            Action act = () =>
                new Product("Product", "desc", 10, 0);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Price must be greater than zero*");
        }

        // 4. Produto inicia ativo
        [Fact]
        public void Should_Create_Product_As_Active()
        {
            var product = new Product("Product", "desc", 10, 10);

            product.Status.Should().Be(ProductStatus.Active);
        }
    }
}