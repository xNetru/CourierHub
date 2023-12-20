using CourierHub.Shared.ApiModels;
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
        private readonly int _serviceId = 1; // to be readen from some memory

        public OrderController(CourierHubDbContext context) {
            _context = context;
        }

        // GET: <OrderController>/30
        [HttpGet("days")]
        public async Task<ActionResult<IEnumerable<ApiOrder>>> Get(int days) {
            DateTime today = DateTime.Now;
            DateTime before = today.AddDays(-days);

            var orders = await _context.Orders.Where(e => e.Inquire.Datetime >= before).ToListAsync();
            if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<ApiOrder>()); }

            var apiOrders = new List<ApiOrder>();
            foreach (var order in orders) {
                apiOrders.Add((ApiOrder)order);
            }
            return Ok(apiOrders);
        }

        // GET: <OrderController>/{...}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiOrder>>> GetConfirmed([FromBody] StatusType? statusType) {
            if (statusType == null) { return BadRequest(); }
            var orders = await _context.Orders.Where(e => e.Service.Name == _serviceName && e.Status.Id == (int)statusType).ToListAsync();
            if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<Order>()); }

            var apiOrders = new List<ApiOrder>();
            foreach (var order in orders) {
                apiOrders.Add((ApiOrder)order);
            }
            return Ok(apiOrders);
            // GET with body???
        }

        // POST: <OrderController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ApiOrder? order) {
            if (order == null) { return BadRequest(); }
            var inquire = await _context.Inquires.FirstOrDefaultAsync(e => e.Code == order.Code);
            if (inquire == null) { return NotFound(); }

            var apiOrders = new List<ApiOrder>();

            var orderDB = (Order)order;
            orderDB.InquireId = inquire.Id;
            orderDB.ServiceId = _serviceId;
            orderDB.StatusId = (int)StatusType.NotConfirmed;

            await _context.Orders.AddAsync(orderDB);
            await _context.SaveChangesAsync();
            return Ok();
            // The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Order_Status".
            // The conflict occurred in database "CourierHubDB", table "dbo.Status", column 'Id'
        }

        /*
        // PUT: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/{...}
        [HttpPut("{code}")]
        public async Task<ActionResult> Put(string code, [FromBody] ApiOrder? order) {
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
        */


        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/status/{...}
        [HttpPatch("{code}/status")]
        public async Task<ActionResult> PatchStatus(string code, [FromBody] StatusType? statusType) {
            if (statusType == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            var status = await _context.Statuses.FirstOrDefaultAsync(e => e.Id == (int)statusType);
            order.StatusId = (int)statusType;
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*
        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/evaluation/{...}
        [HttpPatch("{code}/evaluation")]
        public async Task<ActionResult> PatchEvaluation(string code, [FromBody] ApiEvaluation? evaluation) {
            if (evaluation == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }

            order.Evaluation = evaluation;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/review/{...}
        [HttpPatch("{code}/review")]
        public async Task<ActionResult> PatchReview(string code, [FromBody] ApiReview? review) {
            if (review == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            order.Review = review;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/parcel/{...}
        [HttpPatch("{code}/parcel")]
        public async Task<ActionResult> PatchParcel(string code, [FromBody] ApiParcel? parcel) {
            if (parcel == null) { return BadRequest(); }
            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }
            var status = await _context.Statuses.FirstOrDefaultAsync(e => e.Id == (int)StatusType.PickedUp);
            order.StatusId = (int)StatusType.PickedUp;
            order.Status = status;
            order.Parcel = parcel;
            await _context.SaveChangesAsync();
            return Ok();
        }
        */
    }
}
