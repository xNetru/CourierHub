using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        // GET: <CourierController>/Courier?id=123&email=email@gmail.com
        [HttpGet]
        public async Task<ActionResult<Courier?>> Get(
            [FromQuery(Name = "email")] string? email,
            [FromQuery(Name = "id")] int? id) {

            if (id != null) {
                return Ok((Courier?)await _context.Users.FirstOrDefaultAsync(u => u.Id == id));
            } else if (!email.IsNullOrEmpty()) {
                return Ok((Courier?)await _context.Users.FirstOrDefaultAsync(u => u.Email == email));
            }
            return NotFound(null);
        }
    }
}
