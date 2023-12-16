using CourierHub.Server.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Server.Controllers {
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

        // GET: <UserController>/User?id=123&email=email@gmail.com
        [HttpGet]
        public async Task<ActionResult<User?>> Get(
            [FromQuery(Name = "email")] string? email,
            [FromQuery(Name = "id")] int? id) {

            if (id != null) {
                return Ok(await _context.Users.FirstOrDefaultAsync(u => u.Id == id));
            } else if (!email.IsNullOrEmpty()) {
                return Ok(await _context.Users.FirstOrDefaultAsync(u => u.Email == email));
            }
            return NotFound(null);
        }

        // POST <UserController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] User? user) {
            if (user == null) { return BadRequest(); }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT <UserController>/123
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] User? user) {
            if (user == null) { return BadRequest(); }
            var entity = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) {
                await _context.Users.AddAsync(user);
            } else {
                entity = user;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
