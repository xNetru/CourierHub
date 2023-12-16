using CourierHub.Server.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CourierHub.Server.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public OrderController(CourierHubDbContext context) {
            _context = context;
        }

        // PUT: <OrderController>/123/{...}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Order? order) {
            if (order == null) { return BadRequest(); }
            var entity = await _context.Orders.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) {
                await _context.Orders.AddAsync(order);
            } else {
                entity = order;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
