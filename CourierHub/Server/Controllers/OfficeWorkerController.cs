using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers;
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

    // PATCH: <OfficeWorkerController>/email@gmail.com/order/q1w2-e3r4-t5y6-u7i8-o9p0/evaluation/{...}
    [HttpPatch("{email}/order/{code}/evaluation")]
    public async Task<ActionResult> PatchEvaluation(string email, string code, [FromBody] ApiEvaluation? evaluation) {
        if (evaluation == null) { return BadRequest(); }

        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Type == (int)UserType.OfficeWorker);
        if (user == null) { return NotFound(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var evaluationDB = (Evaluation)evaluation;
        evaluationDB.OfficeWorkerId = user.Id;
        await _context.Evaluations.AddAsync(evaluationDB);
        await _context.SaveChangesAsync();

        order.EvaluationId = evaluationDB.Id;
        await _context.SaveChangesAsync();
        return Ok();
    }

}
