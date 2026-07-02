using FluentValidation;
using OrderManagement.Application.Contracts.Requests;

namespace OrderManagement.Application.Validators.Order
{
    public class CreateOrderItemValidator
        : AbstractValidator<CreateOrderItemRequest>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}
