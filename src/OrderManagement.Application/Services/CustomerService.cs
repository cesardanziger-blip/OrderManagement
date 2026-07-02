using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Mappings;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
        {
            if (await _customerRepository.EmailExistsAsync(request.Email))
                throw new DuplicateCustomerException("email");

            if (await _customerRepository.DocumentExistsAsync(request.Document))
                throw new DuplicateCustomerException("document");


            var customer = request.ToDomain();
            await _customerRepository.CreateAsync(customer);

            return customer.ToResponse();
        }

        public async Task<List<CustomerResponse>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(c => c.ToResponse()).ToList();
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id)
                ?? throw new CustomerNotFoundException(id);

            return customer?.ToResponse();
        }

        public async Task UpdateStatusAsync(Guid id, UpdateCustomerStatusRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id)
                ?? throw new CustomerNotFoundException(id);

            if (request.Active)
                customer.Activate();
            else
                customer.Deactivate();

            await _customerRepository.UpdateAsync(customer);
        }
    }
}
