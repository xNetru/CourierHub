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
                _ = order.ClientAddress;
                apiOrders.Add((ApiOrder)order);
            }
            return Ok(apiOrders);
        }

        // GET: <OrderController>/1/status
        [HttpGet("{status}/status")]
        public async Task<ActionResult<IEnumerable<ApiOrder>>> GetConfirmed(int status) {
            // hardcoded
            if (status < 1 || status > 7) { return BadRequest(); }
            var statusType = (StatusType)Enum.Parse(typeof(StatusType), status.ToString());
            var orders = await _context.Orders.Where(e => e.Service.Name == _serviceName && e.Status.Id == (int)statusType).ToListAsync();
            if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<Order>()); }

            var apiOrders = new List<ApiOrder>();
            foreach (var order in orders) {
                _ = order.ClientAddress;
                apiOrders.Add((ApiOrder)order);
            }
            return Ok(apiOrders);
        }

        // POST: <OrderController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ApiOrder? order) {
            return await AddOrder(order);
            // The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Order_Status".
            // The conflict occurred in database "CourierHubDB", table "dbo.Status", column 'Id'
            // => Trzeba dodać rekordy do tabeli Status
        }


        // PUT: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/{...}
        [HttpPut("{code}")]
        public async Task<ActionResult> Put(string code, [FromBody] ApiOrder? order) {
            if (order == null) { return BadRequest(); }
            var entity = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (entity == null) {
                return await AddOrder(order);
            } else {
                entity.Price = order.Price;
                entity.ClientEmail = order.ClientEmail;
                entity.ClientName = order.ClientName;
                entity.ClientSurname = order.ClientSurname;
                entity.ClientPhone = order.ClientPhone;
                entity.ClientCompany = order.ClientCompany;
                entity.ClientAddress.Street = order.ClientAddress.Street;
                entity.ClientAddress.Number = order.ClientAddress.Number;
                entity.ClientAddress.Flat = order.ClientAddress.Flat;
                entity.ClientAddress.PostalCode = order.ClientAddress.PostalCode;
                entity.ClientAddress.City = order.ClientAddress.City;
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
            order.StatusId = (int)statusType;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/review/{...}
        [HttpPatch("{code}/review")]
        public async Task<ActionResult> PatchReview(string code, [FromBody] ApiReview? review) {
            if (review == null) { return BadRequest(); }

            var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
            if (order == null) { return NotFound(); }

            var reviewDB = (Review)review;
            await _context.Reviews.AddAsync(reviewDB);
            await _context.SaveChangesAsync();

            order.ReviewId = reviewDB.Id;
            await _context.SaveChangesAsync();
            return Ok();
        }


        private async Task<ActionResult> AddOrder(ApiOrder? order) {
            if (order == null) { return BadRequest(); }
            var inquire = await _context.Inquires.FirstOrDefaultAsync(e => e.Code == order.Code);
            if (inquire == null) { return NotFound(); }

            var orderDB = (Order)order;
            orderDB.InquireId = inquire.Id;
            orderDB.ServiceId = _serviceId;
            orderDB.StatusId = (int)StatusType.NotConfirmed;

            await _context.Orders.AddAsync(orderDB);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
