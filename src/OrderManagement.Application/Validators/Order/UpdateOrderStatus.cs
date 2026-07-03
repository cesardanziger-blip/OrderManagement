using FluentValidation;
using OrderManagement.Application.Contracts.Requests;

namespace OrderManagement.Application.Validators.Order
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusRequest>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid order status.");

            RuleFor(x => x.Reason)
                .MaximumLength(250)
                .When(x => x.Reason != null)
                .WithMessage("Reason must be up to 250 characters.");
        }
    }
}
