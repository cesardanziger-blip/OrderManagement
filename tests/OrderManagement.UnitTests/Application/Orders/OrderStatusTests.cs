using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.UnitTests.Application.Orders
{
    public class OrderStatusTests
    {
        // 1. Created -> Paid
        [Fact]
        public void Should_Allow_Pay_From_Created()
        {
            var order = new Order(Guid.NewGuid());

            order.MarkAsPaid();

            order.Status.Should().Be(OrderStatus.Paid);
        }

        // 2. Paid -> Shipped
        [Fact]
        public void Should_Allow_Ship_From_Paid()
        {
            var order = new Order(Guid.NewGuid());

            order.MarkAsPaid();
            order.MarkAsShipped();

            order.Status.Should().Be(OrderStatus.Shipped);
        }

        // 3. Created -> Cancelled
        [Fact]
        public void Should_Allow_Cancel_From_Created()
        {
            var order = new Order(Guid.NewGuid());

            order.MarkAsCancelled();

            order.Status.Should().Be(OrderStatus.Cancelled);
        }

        // 4. Shipped não pode cancelar
        [Fact]
        public void Should_Not_Allow_Cancel_When_Shipped()
        {
            var order = new Order(Guid.NewGuid());

            order.MarkAsPaid();
            order.MarkAsShipped();

            Action act = () => order.MarkAsCancelled();

            act.Should().Throw<InvalidOperationException>();
        }

        // 5. Não pode pagar pedido já pago
        [Fact]
        public void Should_Not_Allow_Pay_Twice()
        {
            var order = new Order(Guid.NewGuid());

            order.MarkAsPaid();

            Action act = () => order.MarkAsPaid();

            act.Should().Throw<InvalidOperationException>();
        }
    }
}