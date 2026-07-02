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
        }
    }
}
