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

        // GET: <InquireController>/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquire>>> Get() {
            var inquires = await _context.Orders.Select(o => o).ToListAsync();
            if (inquires.IsNullOrEmpty()) { return NotFound(Array.Empty<Inquire>()); }
            return Ok(inquires);
        }

        // POST: <InquireController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Inquire? inquire) {
            if (inquire == null) { return BadRequest(); }
            await _context.Inquires.AddAsync(inquire);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}