using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCustomerRequest request)
        {
            var id = await _customerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // GET: api/customers/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetById(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<List<CustomerResponse>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        // PATCH: api/customers/{id}/status
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            await _customerService.DeactivateAsync(id);
            return NoContent();
        }
    }
}