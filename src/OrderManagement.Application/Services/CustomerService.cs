using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Exceptions.Customer;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Mappings;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
        {
            if (await _customerRepository.EmailExistsAsync(request.Email))
                throw new DuplicateCustomerException("email");

            if (await _customerRepository.DocumentExistsAsync(request.Document))
                throw new DuplicateCustomerException("document");


            var customer = request.ToDomain();
            await _customerRepository.CreateAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return customer.ToResponse();
        }

        public async Task<PagedResponse<CustomerResponse>> GetAllAsync(PagedRequest request)
        {
            var query = await _customerRepository.GetAllAsync();

            var totalCount = query.Count();

            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => o.ToResponse())
                .ToList();

            return new PagedResponse<CustomerResponse>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CustomerResponse> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id)
                ?? throw new CustomerNotFoundException(id);

            return customer.ToResponse();
        }

        public async Task UpdateStatusAsync(Guid id, UpdateCustomerStatusRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id)
                ?? throw new CustomerNotFoundException(id);

            if (request.Active)
                customer.Activate();
            else
                customer.Deactivate();

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
