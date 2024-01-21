using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Shared.Controllers;
[ApiController]
[Route("[controller]")]
public class InquireController : ControllerBase {
    private readonly CourierHubDbContext _context;

    public InquireController(CourierHubDbContext context) {
        _context = context;
    }

    // GET: <InquireController>/30
    [HttpGet("{days}")]
    public async Task<ActionResult<IEnumerable<ApiInquire>>> Get(int days) {
        DateTime today = DateTime.Now;
        DateTime before = today.AddDays(-days);

        var inquires = await _context.Inquires.Where(e => e.Datetime >= before).ToListAsync();
        if (inquires.IsNullOrEmpty()) { return NotFound(Array.Empty<Inquire>()); }

        var apiInquires = new List<ApiInquire>();
        foreach (var inquire in inquires) {
            inquire.Source = (await _context.Addresses.FirstOrDefaultAsync(e => e.Id == inquire.SourceId))!;
            inquire.Destination = (await _context.Addresses.FirstOrDefaultAsync(e => e.Id == inquire.DestinationId))!;
            apiInquires.Add((ApiInquire)inquire);
        }
        return Ok(apiInquires);
    }

    // GET: <InquireController>/q1w2-e3r4-t5y6-u7i8-o9p0/code
    [HttpGet("{code}/code")]
    public async Task<ActionResult<ApiInquire>> GetInquireByCode(string code) {
        if (code.IsNullOrEmpty()) { return BadRequest(); }

        var inquire = await _context.Inquires.FirstOrDefaultAsync(e => e.Code == code);
        if (inquire == null) { return NotFound(); }

        var source = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == inquire.SourceId);
        if (source == null) { return NotFound(); }

        var destination = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == inquire.DestinationId);
        if (destination == null) { return NotFound(); }

        inquire.Source = source;
        inquire.Destination = destination;

        return Ok((ApiInquire)inquire);
    }

    /* 
     * === UNUSED ===
     * 
    // POST: <InquireController>/email@gmail.com/{...}
    [HttpPost("{email}")]
    public async Task<ActionResult<int>> Post(string email, [FromBody] ApiInquire? inquire) {
        if (inquire == null) { return BadRequest(0); }

        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
        if (user == null) { return NotFound(0); }

        var inquireDB = (Inquire)inquire;
        inquireDB.ClientId = user.Id;
        await _context.Inquires.AddAsync(inquireDB);

        await _context.SaveChangesAsync();
        return Ok(inquireDB.Id);
    }
    */
}