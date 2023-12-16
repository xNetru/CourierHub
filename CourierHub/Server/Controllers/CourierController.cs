using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Server.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CourierController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public CourierController(CourierHubDbContext context) {
            _context = context;
        }

        // HEAD: <CourierController>/email@gmail.com
        [HttpHead("{email}")]
        public async Task<IActionResult> Head(string email) {
            var courier = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Courier);
            if (courier != null) { return Ok(); }
            return NotFound();
        }

        // GET: <CourierController>/email@gmail.com
        [HttpGet("{email}")]
        public async Task<Courier?> Get(string email) {
            return (Courier?)await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Courier);
        }

        // GET: <CourierController>/id
        [HttpGet("{id}")]
        public async Task<Courier?> Get(int id) {
            return (Courier?)await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Courier);
        }
    }
}
