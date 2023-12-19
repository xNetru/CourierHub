using CourierHub.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public UserController(CourierHubDbContext context) {
            _context = context;
        }

        // HEAD: <UserController>/email@gmail.com
        [HttpHead("{email}")]
        public async Task<ActionResult> Head(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) { return Ok(); }
            return NotFound();
        }
    }
}
