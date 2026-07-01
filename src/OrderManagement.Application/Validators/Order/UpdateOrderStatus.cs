using FluentValidation;

namespace OrderManagement.Application.Validators.Order
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required.");

            RuleFor(x => x.NewStatus)
                .IsInEnum()
                .WithMessage("Invalid order status.");

            RuleFor(x => x.Reason)
                .MaximumLength(250)
                .When(x => x.Reason != null)
                .WithMessage("Reason must be up to 250 characters.");
        }
    }
}
