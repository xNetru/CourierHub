using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers {
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
    }
}
