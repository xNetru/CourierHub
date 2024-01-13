using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared.Controllers;
[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase {
    private readonly CourierHubDbContext _context;
    private readonly InquireCodeContainer _container;
    private readonly IEnumerable<IWebApi> _webApis;

    public ApiController(CourierHubDbContext context, IConfiguration config, InquireCodeContainer container) {
        _context = context;
        _container = container;
        // w przyszłości z bazy danych, na czas testów z configa
        // -----
        string adres = config.GetValue<string>("ApiAddress") ??
            throw new NullReferenceException("Base address could not be loaded!");
        var service = new ApiService {
            Name = "CourierHub",
            ApiKey = "1fbbdd4f48fb4c87890cef420d865b86",
            BaseAddress = adres
        };
        // -----
        var webApis = new List<IWebApi> {
            new CourierHubApi(service)
        };
        _webApis = webApis;
    }

    // POST: <ApiController>/inquire/{...}
    [HttpPost("inquire")]
    public async Task<ActionResult<IEnumerable<ApiOffer>>> PostInquireGetOffers([FromBody] ApiInquire? inquire) {
        if (inquire == null) { return BadRequest(Array.Empty<ApiOffer>()); }

        var offers = new List<ApiOffer>();
        foreach (var webapi in _webApis) {
            (ApiOffer? offer, int status) = await webapi.PostInquireGetOffer(inquire);
            if (offer != null && status >= 200 && status < 300) {
                offers.Add(offer);
            }
        }

        if (offers.Any()) {
            Inquire inquireDB = (Inquire)inquire;
            if (inquire.Email != null) {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == inquire.Email && e.Type == (int)UserType.Client);
                if (user != null) {
                    inquireDB.ClientId = user.Id;
                }
            }
            await _context.Inquires.AddAsync(inquireDB);
            await _context.SaveChangesAsync();

            // cash inquire with codes
            var codeList = offers.Select(e => e.Code).ToList();
            _container.InquireCodes.Add((codeList, inquireDB.Id));

            return Ok(offers);
        } else {
            return NotFound(Array.Empty<ApiOffer>());
        }
    }

    // POST: <ApiController>/CourierHub/order/{...}
    [HttpPost("{serviceName}/order")]
    public async Task<ActionResult> PostOrder(string serviceName, [FromBody] ApiOrder? order) {
        if (order == null) { return BadRequest(); }

        var service = await _context.Services.FirstOrDefaultAsync(e => e.Name == serviceName);
        if (service == null) { return NotFound(); }

        foreach (var webapi in _webApis) {
            if (webapi.ServiceName == serviceName) {
                int status = await webapi.PostOrder(order);

                // retrieve cashed id
                int inquireId = _container.InquireCodes.FirstOrDefault(e => e.Item1.Contains(order.Code)).Item2;

                var inquireDB = _context.Inquires.FirstOrDefault(e => e.Id == inquireId);
                if (inquireDB == null) { return NotFound(); }
                // for now this code, but in case of e.g. SzymoAPI it must be retrived from webapi.PostOrder(order)
                inquireDB.Code = order.Code;

                Order orderDB = (Order)order;
                orderDB.InquireId = inquireId;
                orderDB.ServiceId = service.Id;
                orderDB.StatusId = (int)StatusType.NotConfirmed;

                await _context.Orders.AddAsync(orderDB);
                await _context.SaveChangesAsync();

                return StatusCode(status);
            }
        }
        return NotFound(); // should not happen if serviceName exists
    }

    // PATCH: <ApiController>/CourierHub/cancel/q1w2-e3r4-t5y6-u7i8-o9p0
    [HttpPatch("{serviceName}/cancel/{code}")]
    public async Task<ActionResult> PutOrderWithrawal(string serviceName, string code) {
        foreach (var webapi in _webApis) {
            if (webapi.ServiceName == serviceName) {
                int status = await webapi.PutOrderWithrawal(code);
                return StatusCode(status);
            }
        }
        return NotFound(); // should not happen if serviceName exists
    }

    /* 
     * === UNUSED ===
     * 
    // GET: <ApiController>/CourierHub/status/q1w2-e3r4-t5y6-u7i8-o9p0
    [HttpGet("{serviceName}/status/{code}")]
    public async Task<ActionResult<StatusType?>> GetOrderStatus(string serviceName, string code) {
        foreach (var webapi in _webApis) {
            if (webapi.ServiceName == serviceName) {
                (StatusType? type, int status) = await webapi.GetOrderStatus(code);
                if (type != null && status >= 200 && status < 300) {
                    return Ok(type);
                } else {
                    return BadRequest(type);
                }
            }
        }
        return NotFound(null); // should not happen if serviceName exists
    }
    */
}
