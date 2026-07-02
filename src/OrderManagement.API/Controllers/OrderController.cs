using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.Requests;
using OrderManagement.Application.Contracts.Responses;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderRequest request)
        {
            var id = await _orderService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Returns all orders.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderResponse>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Returns an order by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponse>> GetById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order is null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Updates order status (Paid, Shipped, Cancelled).
        /// </summary>
        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatus(Guid id,[FromBody] UpdateOrderStatusRequest request)
        {
            await _orderService.UpdateStatusAsync(id, request);

            return NoContent();
        }
    }
}