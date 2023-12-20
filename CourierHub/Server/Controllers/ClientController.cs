using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var client = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Client);
            if (client != null) { return Ok(); }
            return NotFound();
        }

        // GET: <ClientController>/email@gmail.com
        [HttpGet("{email}")]
        public async Task<ActionResult<ApiClient?>> Get(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Client);
            if (user == null) { return NotFound(null); }

            var data = await _context.ClientDatum.FirstOrDefaultAsync(e => e.ClientId == user.Id);
            if (data == null) { return NotFound(null); }

            var address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == data.AddressId);
            var sourceAddress = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == data.SourceAddressId);

            return Ok(new ApiClient() {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Photo = data.Photo,
                Phone = data.Phone,
                Company = data.Company,
                Address = (ApiAddress)address,
                SourceAddress = (ApiAddress)sourceAddress
            });
        }

        // POST <ClientController>/{...}
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ApiClient? client) {
            return await AddClient(client);
        }

        // PUT <ClientController>/email@gmail.com
        [HttpPut("{email}")]
        public async Task<ActionResult> Put(string email, [FromBody] ApiClient? client) {
            if (client == null) { return BadRequest(); }
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Client);
            if (user == null) {
                return await AddClient(client);
            } else {
                var data = await _context.ClientDatum.FirstOrDefaultAsync(e => e.ClientId == user.Id);

                user.Email = client.Email;
                user.Name = client.Name;
                user.Surname = client.Surname;
                data.Photo = client.Photo;
                data.Phone = client.Phone;
                data.Company = client.Company;

                data.Address.Street = client.Address.Street;
                data.Address.Number = client.Address.Number;
                data.Address.Flat = client.Address.Flat;
                data.Address.PostalCode = client.Address.PostalCode;
                data.Address.City = client.Address.City;

                data.SourceAddress.Street = client.SourceAddress.Street;
                data.SourceAddress.Number = client.SourceAddress.Number;
                data.SourceAddress.Flat = client.SourceAddress.Flat;
                data.SourceAddress.PostalCode = client.SourceAddress.PostalCode;
                data.SourceAddress.City = client.SourceAddress.City;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: <ClientController>/email@gmail.com/inquires
        [HttpGet("{email}/inquires/{days}")]
        public async Task<ActionResult<IEnumerable<Inquire>>> GetInquires(string email, int days) {
            DateTime today = DateTime.Now;
            DateTime before = today.AddDays(-days);

            var client = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Client);
            if (client == null) { return NotFound(Array.Empty<Inquire>()); }
            return Ok(await _context.Inquires.Where(e => e.ClientId == client.Id && e.Datetime >= before).ToListAsync());
            // System.Text.Json.JsonException: A possible object cycle was detected.
            // This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
            // Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
            // Path: $.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Client.Inquires.Id.
        }

        // GET: <ClientController>/email@gmail.com/orders
        [HttpGet("{email}/orders/{days}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string email, int days) {
            DateTime today = DateTime.Now;
            DateTime before = today.AddDays(-days);

            var client = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.Client);
            if (client == null) { return NotFound(Array.Empty<Order>()); }
            return Ok(await _context.Orders.Where(e => e.Inquire.ClientId == client.Id && e.Inquire.Datetime >= before).ToListAsync());
        }

        private async Task<ActionResult> AddClient(ApiClient? client) {
            if (client == null) { return BadRequest(); }

            var user = (User)client;
            await _context.Users.AddAsync(user);
            var userDB = _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);

            var data = (ClientData)client;
            data.ClientId = userDB.Id;
            await _context.ClientDatum.AddAsync(data);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
