using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers;
[ApiController]
[Route("[controller]")]
public class CourierController : ControllerBase {
    private readonly CourierHubDbContext _context;

    public CourierController(CourierHubDbContext context) {
        _context = context;
    }

    // HEAD: <CourierController>/email@gmail.com
    [HttpHead("{email}")]
    public async Task<ActionResult> Head(string email) {
        var courier = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Courier);
        if (courier != null) { return Ok(); }
        return NotFound();
    }

    // GET: <CourierController>/email@gmail.com
    [HttpGet("{email}")]
    public async Task<ActionResult<ApiCourier?>> Get(string email) {
        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Courier);
        if (user == null) { return NotFound(null); }
        return Ok((ApiCourier)user);
    }

    // PATCH: <CourierController>/email@gmail.com/order/q1w2-e3r4-t5y6-u7i8-o9p0/parcel/{...}
    [HttpPatch("{email}/order/{code}/parcel")]
    public async Task<ActionResult> PatchParcel(string email, string code, [FromBody] ApiParcel? parcel) {
        if (parcel == null) { return BadRequest(); }

        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Courier);
        if (user == null) { return NotFound(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var parcelDB = (Parcel)parcel;
        parcelDB.CourierId = user.Id;
        if (order.ParcelId != null) {
            order.Parcel = parcelDB;
            order.Parcel.Id = order.ParcelId.Value;
        } else {
            await _context.Parcels.AddAsync(parcelDB);
        }
        await _context.SaveChangesAsync();

        order.ParcelId = parcelDB.Id;
        order.StatusId = (int)StatusType.PickedUp;
        await _context.SaveChangesAsync();
        return Ok();
    }
}
