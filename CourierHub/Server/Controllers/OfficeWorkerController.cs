using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OfficeWorkerController : ControllerBase {
        private readonly CourierHubDbContext _context;

        public OfficeWorkerController(CourierHubDbContext context) {
            _context = context;
        }

        // HEAD: <OfficeWorkerController>/email@gmail.com
        [HttpHead("{email}")]
        public async Task<ActionResult> Head(string email) {
            var worker = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == (int)UserType.OfficeWorker);
            if (worker != null) { return Ok(); }
            return NotFound();
        }

        // GET: <OfficeWorkerController>/email@gmail.com
        [HttpGet("{email}")]
        public async Task<ActionResult<ApiOfficeWorker?>> Get(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.OfficeWorker);
            if (user == null) { return NotFound(null); }
            return Ok((ApiOfficeWorker)user);
        }
    }
}
