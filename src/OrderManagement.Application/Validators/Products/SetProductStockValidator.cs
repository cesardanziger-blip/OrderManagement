using FluentValidation;
using OrderManagement.Application.Contracts.Requests;

namespace OrderManagement.Application.Validators.Products
{
    public class SetProductStockValidator : AbstractValidator<SetProductStockRequest>
    {
        public SetProductStockValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock cant be negative");
        }
    }
}
