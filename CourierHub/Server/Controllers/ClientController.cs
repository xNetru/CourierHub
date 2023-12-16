using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CourierHub.Server.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public ClientController(CourierHubDbContext context) {
            _context = context;
        }

        // HEAD: <ClientController>/email@gmail.com
        [HttpHead("{email}")]
        public async Task<IActionResult> Head(string email) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Client);
            if (client != null) { return Ok(); }
            return NotFound();
        }

        // GET: <ClientController>/email@gmail.com
        [HttpGet("{email}")]
        public async Task<Shared.Models.Client?> Get(string email) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Client);
            if (client == null) { return null; }
            var data = await _context.ClientDatum.FirstOrDefaultAsync(d => d.ClientId == client.Id);
            if (data == null) { return null; }
            return new Shared.Models.Client() {
                Data = data
            };
        }

        // GET: <ClientController>/id
        [HttpGet("{id}")]
        public async Task<Shared.Models.Client?> Get(int id) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            if (client == null) { return null; }
            var data = await _context.ClientDatum.FirstOrDefaultAsync(d => d.ClientId == client.Id);
            if (data == null) { return null; }
            return new Shared.Models.Client() {
                Data = data
            };
        }

        // POST <ClientController>/{...}
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value) {
            var client = (Shared.Models.Client?)JsonSerializer.Deserialize(value, typeof(Shared.Models.Client));
            if (client == null) { return BadRequest(); }
            await _context.Users.AddAsync(client);
            ClientData data = client.Data;
            await _context.ClientDatum.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT <UserController>/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value) {
            var client = (Shared.Models.Client?)JsonSerializer.Deserialize(value, typeof(Shared.Models.Client));
            if (client == null) { return BadRequest(); }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            var data = await _context.ClientDatum.FirstOrDefaultAsync(d => d.ClientId == id);
            if (user == null) {
                await _context.Users.AddAsync(client);
                await _context.ClientDatum.AddAsync(client.Data);
            } else {
                user = client;
                data = client.Data;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: <ClientController>/id/inquires
        [HttpGet("{id}/inquires")]
        public async Task<IEnumerable<Inquire>> GetInquires(int id) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            if (client == null) { return Array.Empty<Inquire>(); }
            return await _context.Inquires.Where(i => i.ClientId == client.Id).ToListAsync();
        }

        // GET: <ClientController>/id/orders
        [HttpGet("{id}/inquires")]
        public async Task<IEnumerable<Order>> GetOrders(int id) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            if (client == null) { return Array.Empty<Order>(); }
            return await _context.Orders.Where(o => o.Inquire.ClientId == client.Id).ToListAsync();
        }
    }
}
