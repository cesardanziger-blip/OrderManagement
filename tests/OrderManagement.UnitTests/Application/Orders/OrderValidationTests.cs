using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.UnitTests.Application.Orders
{
    public class OrderValidationTests
    {
        // 1. Pedido inicia como Created
        [Fact]
        public void Should_Create_Order_With_Created_Status()
        {
            var order = new Order(Guid.NewGuid());

            order.Status.Should().Be(OrderStatus.Created);
        }

        // 2. Só adiciona item em Created
        [Fact]
        public void Should_Allow_Add_Item_Only_When_Created()
        {
            var order = new Order(Guid.NewGuid());

            var product = new Product("Mouse", "desc", 10, 100);

            order.AddItem(product, 1);

            order.Items.Should().HaveCount(1);
        }

        // 3. Não pode adicionar item após pagamento
        [Fact]
        public void Should_Not_Allow_Add_Item_When_Not_Created()
        {
            var order = new Order(Guid.NewGuid());

            var product = new Product("Mouse", "desc", 10, 100);

            order.MarkAsPaid();

            Action act = () => order.AddItem(product, 1);

            act.Should().Throw<InvalidOperationException>();
        }

        // 4. Total é calculado corretamente
        [Fact]
        public void Should_Calculate_Total_Correctly()
        {
            var order = new Order(Guid.NewGuid());

            var product = new Product("Mouse", "desc", 10, 100);

            order.AddItem(product, 2);

            order.Total.Should().Be(200);
        }
    }
}