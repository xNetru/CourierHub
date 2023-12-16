using CourierHub.Server.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
        public async Task<IActionResult> Head(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) { return Ok(); }
            return NotFound();
        }

        // GET: <UserController>/User?email=email@gmail.com
        [HttpGet]
        public async Task<User?> GetByEmail([FromQuery] string email) {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // GET: <UserController>/User?id=123
        [HttpGet]
        public async Task<User?> GetById([FromQuery] int id) {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        // POST <UserController>/{...}
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value) {
            var user = (User?)JsonSerializer.Deserialize(value, typeof(User));
            if (user == null) { return BadRequest(); }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT <UserController>/123
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value) {
            var user = (User?)JsonSerializer.Deserialize(value, typeof(User));
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
