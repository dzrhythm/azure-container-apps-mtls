using Microsoft.AspNetCore.Mvc;
using WidgetOrderService.Data;
using WidgetOrderService.Entities;
using WidgetOrderService.Models;

namespace WidgetOrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _repository;
        private readonly ILogger _logger;

        public OrdersController(OrderRepository repository, ILogger<OrdersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> GetOrder(long id)
        {
            Order? order = await _repository.GetAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> Create(OrderDto orderRequest)
        {
            Order order = await _repository.AddAsync(new Order { Count = orderRequest.Count });

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        private void LogRequest()
        {
            foreach (var header in Request.Headers)
            {
                _logger.LogInformation("Header {Key}={Value}", header.Key, header.Value);
            }
        }
    }
}
