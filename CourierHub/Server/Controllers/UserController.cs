using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers;
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

    // GET: <CourierController>/email@gmail.com
    [HttpGet("{email}")]
    public async Task<ActionResult<UserType?>> GetType(string email) {
        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
        if (user == null) { return NotFound(null); }
        return Ok((UserType)Enum.Parse(typeof(UserType), user.Type.ToString()));
    }
}