using FluentAssertions;
using OrderManagement.Domain.Entities;

namespace OrderManagement.UnitTests.Application
{
    public class CustomerValidationTests
    {
        //----> 1. nome vazio
        [Fact]
        public void Should_Throw_When_Name_Is_Empty()
        {
            Action act = () =>
                new Customer("", "test@test.com", "123");

            act.Should().Throw<ArgumentException>();
        }

        //----> 2. email vazio
        [Fact]
        public void Should_Throw_When_Email_Is_Empty()
        {
            Action act = () =>
                new Customer("John", "", "123");

            act.Should().Throw<ArgumentException>();
        }

        //----> 2. document vazio
        [Fact]
        public void Should_Throw_When_Document_Is_Empty()
        {
            Action act = () =>
                new Customer("John", "test@test.com", "");

            act.Should().Throw<ArgumentException>();
        }

    }
}
