using CourierHub.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CourierHub.Shared.Models;
using SM = CourierHub.Shared.Models;

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
            if (user != null) { return Ok(); } else {
                return NotFound();
            }
        }

        // consider different endpoint
        // GET: <UserController>/email@gmail.com
        /*
        [HttpGet("{email}")]
        public async Task<IEnumerable<int>> Get(string email) {
            var client = await _context.Clients.FirstOrDefaultAsync(cl => cl.Email == email);
            if (client != null) { return new int[] { 1 }; } else {
                var worker = await _context.OfficeWorkers.FirstOrDefaultAsync(wo => wo.Email == email);
                if (worker != null) { return new int[] { 2 }; } else {
                    var courier = await _context.Couriers.FirstOrDefaultAsync(co => co.Email == email);
                    if (courier != null) { return new int[] { 3 }; }
                }
            }
            return new int[] { 0 };
        }
        */

        // POST <UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value) {
            SM.Client? client = (SM.Client?)JsonSerializer.Deserialize(value, typeof(SM.Client));
            if (client == null) { return BadRequest(); }
            // create user
            // create client data
            // add both
            //await _context.Clients.AddAsync(client);
            //await _context.SaveChangesAsync();
            return Ok();
        }

        /*
        // GET: <UserController>
        [HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET <UserController>/5
        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }
        */

        /*
        // PUT <UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE <UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
        */
    }
}
