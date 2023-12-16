using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public ClientController(CourierHubDbContext context) {
            _context = context;
        }

        // HEAD: <ClientController>/email@gmail.com
        [HttpHead("{email}")]
        public async Task<ActionResult> Head(string email) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Client);
            if (client != null) { return Ok(); }
            return NotFound();
        }

        // GET: <ClientController>/Client?id=123&email=email@gmail.com
        [HttpGet]
        public async Task<ActionResult<Shared.Models.Client?>> Get(
            [FromQuery(Name = "email")] string? email,
            [FromQuery(Name = "id")] int? id) {

            User? client = null;
            if (id != null) {
                client = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            } else if (!email.IsNullOrEmpty()) {
                client = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.Client);
            }
            if (client == null) { return NotFound(null); }

            var data = await _context.ClientDatum.FirstOrDefaultAsync(d => d.ClientId == client.Id);
            if (data == null) { return NotFound(null); }
            return Ok(new Shared.Models.Client() { Data = data });
        }

        // POST <ClientController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Shared.Models.Client? client) {
            if (client == null) { return BadRequest(); }
            await _context.Users.AddAsync(client);
            ClientData data = client.Data;
            await _context.ClientDatum.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT <ClientController>/123
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Shared.Models.Client? client) {
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

        // GET: <ClientController>/123/inquires
        [HttpGet("{id}/inquires")]
        public async Task<ActionResult<IEnumerable<Inquire>>> GetInquires(int id) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            if (client == null) { return NotFound(Array.Empty<Inquire>()); }
            return Ok(await _context.Inquires.Where(i => i.ClientId == client.Id).ToListAsync());
        }

        // GET: <ClientController>/123/orders
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int id) {
            var client = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.Client);
            if (client == null) { return NotFound(Array.Empty<Order>()); }
            return Ok(await _context.Orders.Where(o => o.Inquire.ClientId == client.Id).ToListAsync());
        }
    }
}
