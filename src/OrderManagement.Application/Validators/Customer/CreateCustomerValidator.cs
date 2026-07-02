using FluentValidation;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Validators.Common;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Validators.Customer
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerValidator(ICustomerRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Document)
                .NotEmpty()
                .Must(CpfCnpjValidator.IsValidCpfOrCnpj)
                .WithMessage("Invalid CPF or CNPJ.");

            RuleFor(x => x)
                .MustAsync(BeUniqueEmail)
                .WithMessage("An active customer with this email already exists.");

            RuleFor(x => x)
                .MustAsync(BeUniqueDocument)
                .WithMessage("An active customer with this document already exists.");
        }

        private async Task<bool> BeUniqueEmail(CreateCustomerRequest request, CancellationToken ct)
        {
            return !await _repository.EmailExistsAsync(request.Email);
        }

        private async Task<bool> BeUniqueDocument(CreateCustomerRequest request, CancellationToken ct)
        {
            return !await _repository.DocumentExistsAsync(request.Document);
        }
    }
}
