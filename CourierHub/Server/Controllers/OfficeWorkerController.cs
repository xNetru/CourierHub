using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: <OfficeWorkerController>/email@gmail.com
        [HttpGet("{email}")]
        public async Task<OfficeWorker?> Get(string email) {
            return (OfficeWorker?)await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.OfficeWorker);
        }

        // GET: <OfficeWorkerController>/id
        [HttpGet("{id}")]
        public async Task<OfficeWorker?> Get(int id) {
            return (OfficeWorker?)await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Type == (int)UserType.OfficeWorker);
        }
    }
}
