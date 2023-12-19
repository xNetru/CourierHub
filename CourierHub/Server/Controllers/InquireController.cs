using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class InquireController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public InquireController(CourierHubDbContext context) {
            _context = context;
        }

        // GET: <InquireController>/30
        [HttpGet("days")]
        public async Task<ActionResult<IEnumerable<ApiInquire>>> Get(int days) {
            DateTime today = DateTime.Now;
            DateTime before = today.AddDays(-days);

            var inquires = await _context.Inquires.Where(e => e.Datetime >= before).ToListAsync();
            if (inquires.IsNullOrEmpty()) { return NotFound(Array.Empty<Inquire>()); }

            var apiInquires = new List<ApiInquire>();
            foreach(var inquire in inquires) {
                apiInquires.Add((ApiInquire)inquire);
            }
            return Ok(apiInquires);
        }

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
            return Ok();
        }
    }
}