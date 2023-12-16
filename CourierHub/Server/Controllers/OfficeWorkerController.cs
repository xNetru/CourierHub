using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourierHub.Server.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OfficeWorkerController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public OfficeWorkerController(CourierHubDbContext context) {
            _context = context;
        }

        // HEAD: <OfficeWorkerController>/email@gmail.com
        [HttpHead("{email}")]
        public async Task<IActionResult> Head(string email) {
            var worker = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.OfficeWorker);
            if (worker != null) { return Ok(); }
            return NotFound();
        }

        // GET: <OfficeWorkerController>/OfficeWorker?id=123&email=email@gmail.com
        [HttpGet]
        public async Task<OfficeWorker?> Get(
            [FromQuery(Name = "email")] string? email,
            [FromQuery(Name = "id")] int? id) {

            if (id != null) {
                return (OfficeWorker?)await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            } else if (!email.IsNullOrEmpty()) {
                return (OfficeWorker?)await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            return null;
        }
    }
}
