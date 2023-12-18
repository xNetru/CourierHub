using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase {
        private readonly CourierHubDbContext _context;
        private readonly string _serviceName = "OUR_SERVICE"; // to be readen from some memory

        public OrderController(CourierHubDbContext context) {
            _context = context;
        }

        // GET: <OrderController>/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get() {
            var orders = await _context.Orders.Where(o => o.Service.Name == _serviceName).ToListAsync();
            if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<Order>()); }
            return Ok(orders);
        }

        // GET: <OrderController>/confirmed
        [HttpGet("confirmed")]
        public async Task<ActionResult<IEnumerable<Order>>> GetConfirmed() {
            var orders = await _context.Orders.Where(o => o.Service.Name == _serviceName && o.Status.Id == (int)StatusType.Confirmed).ToListAsync();
            if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<Order>()); }
            return Ok(orders);
        }

        // POST: <OrderController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order? order) {
            if (order == null) { return BadRequest(); }
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/{...}
        [HttpPut("{code}")]
        public async Task<ActionResult> Put(string code, [FromBody] Order? order) {
            if (order == null) { return BadRequest(); }
            var entity = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (entity == null) {
                await _context.Orders.AddAsync(order);
            } else {
                entity = order;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/status/{...}
        [HttpPatch("{code}/status")]
        public async Task<ActionResult> PatchStatus(string code, [FromBody] StatusType? statusType) {
            if (statusType == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            var status = await _context.Statuses.FirstOrDefaultAsync(e => e.Id == (int)statusType);
            order.Status = status;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/evaluation/{...}
        [HttpPatch("{code}/evaluation")]
        public async Task<ActionResult> PatchEvaluation(string code, [FromBody] Evaluation? evaluation) {
            if (evaluation == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            order.Evaluation = evaluation;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/review/{...}
        [HttpPatch("{code}/review")]
        public async Task<ActionResult> PatchReview(string code, [FromBody] Review? review) {
            if (review == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            order.Review = review;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/parcel/{...}
        [HttpPatch("{code}/parcel")]
        public async Task<ActionResult> PatchParcel(string code, [FromBody] Parcel? parcel) {
            if (parcel == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            var status = await _context.Statuses.FirstOrDefaultAsync(e => e.Id == (int)StatusType.PickedUp);
            order.Status = status;
            order.Parcel = parcel;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
