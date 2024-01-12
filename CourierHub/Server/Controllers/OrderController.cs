using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Shared.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase {
    private readonly CourierHubDbContext _context;
    private readonly string _serviceName;

    public OrderController(CourierHubDbContext context, IConfiguration config) {
        _context = context;
        _serviceName = config.GetValue<string>("ServiceName") ??
            throw new NullReferenceException("Service name could not be loaded!");
    }

    // GET: <OrderController>/30
    [HttpGet("{days}")]
    public async Task<ActionResult<IEnumerable<ApiOrder>>> Get(int days) {
        DateTime today = DateTime.Now;
        DateTime before = today.AddDays(-days);

        var orders = await _context.Orders.Where(e => e.Service.Name == _serviceName && e.Inquire.Datetime >= before).ToListAsync();
        if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<ApiOrder>()); }

        var apiOrders = new List<ApiOrder>();
        foreach (var order in orders) {
            order.ClientAddress = (await _context.Addresses.FirstOrDefaultAsync(e => e.Id == order.ClientAddressId))!;
            apiOrders.Add((ApiOrder)order);
        }
        return Ok(apiOrders);
    }

    // GET: <OrderController>/1/order
    [HttpGet("{status}/order")]
    public async Task<ActionResult<IEnumerable<ApiOrder>>> GetOrderByStatus(int status) {
        // hardcoded
        if (status < 1 || status > 7) { return BadRequest(); }
        var orders = await _context.Orders.Where(e => e.Service.Name == _serviceName && e.StatusId == status).ToListAsync();
        if (orders.IsNullOrEmpty()) { return NotFound(Array.Empty<Order>()); }

        var apiOrders = new List<ApiOrder>();
        foreach (var order in orders) {
            order.ClientAddress = (await _context.Addresses.FirstOrDefaultAsync(e => e.Id == order.ClientAddressId))!;
            apiOrders.Add((ApiOrder)order);
        }
        return Ok(apiOrders);
    }

    // GET: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/status
    [HttpGet("{code}/status")]
    public async Task<ActionResult<ApiStatus>> GetStatusByCode(string code) {
        if (code.IsNullOrEmpty()) { return BadRequest(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var status = await _context.Statuses.FirstOrDefaultAsync(e => e.Id == order.StatusId);
        if (status == null) { return NotFound(); }

        return Ok(new ApiStatus { 
            Name = status.Name, IsCancelable = status.IsCancelable
        });
    }

    // GET: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/service
    [HttpGet("{code}/service")]
    public async Task<ActionResult<string>> GetServiceByCode(string code) {
        if (code.IsNullOrEmpty()) { return BadRequest(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var service = await _context.Services.FirstOrDefaultAsync(e => e.Id == order.ServiceId);
        if (service == null) { return NotFound(); }

        return Ok(service.Name);
    }

    // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/status/{...}
    [HttpPatch("{code}/status")]
    public async Task<ActionResult> PatchStatus(string code, [FromBody] StatusType? statusType) {
        if (statusType == null) { return BadRequest(); }
        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }
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

    /* 
     * === UNUSED ===
     * 
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

    // POST: <OrderController>/{...}
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ApiOrder? order) {
        return await AddOrder(order);
    }

    // PUT: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/{...}
    [HttpPut("{code}")]
    public async Task<ActionResult> Put(string code, [FromBody] ApiOrder? order) {
        if (order == null) { return BadRequest(); }
        var entity = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (entity == null) {
            return await AddOrder(order);
        } else {
            var address = (await _context.Addresses.FirstOrDefaultAsync(e => e.Id == entity.ClientAddressId))!;

            entity.Price = order.Price;
            entity.ClientEmail = order.ClientEmail;
            entity.ClientName = order.ClientName;
            entity.ClientSurname = order.ClientSurname;
            entity.ClientPhone = order.ClientPhone;
            entity.ClientCompany = order.ClientCompany;
            entity.ClientAddress.Street = address.Street;
            entity.ClientAddress.Number = address.Number;
            entity.ClientAddress.Flat = address.Flat;
            entity.ClientAddress.PostalCode = address.PostalCode;
            entity.ClientAddress.City = address.City;
        }
        await _context.SaveChangesAsync();
        return Ok();
    }
    */
}
