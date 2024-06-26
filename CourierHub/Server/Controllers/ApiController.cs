﻿using CourierHub.Server.Api;
using CourierHub.Server.Containers;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CourierHub.Shared.Controllers;
[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase {
    private readonly CourierHubDbContext _context;
    private readonly IList<(List<string>, int)> _inquireCodes;
    private readonly IList<IWebApi> _webApis;
    public ApiController(CourierHubDbContext context, WebApiContainer apiContainer, InquireCodeContainer inquireContainer) {
        _context = context;
        _inquireCodes = inquireContainer.InquireCodes;
        _webApis = apiContainer.WebApis;
    }

    // POST: <ApiController>/inquire/{...}
    [HttpPost("inquire")]
    public async Task<ActionResult<IEnumerable<ApiOffer>>> PostInquireGetOffers([FromBody] ApiInquire? inquire) {
        if (inquire == null) { return BadRequest(Array.Empty<ApiOffer>()); }

        var offers = new ConcurrentBag<ApiOffer>();
        var options = new ParallelOptions { MaxDegreeOfParallelism = 3 };
        await Parallel.ForEachAsync(_webApis, options, async (webapi, token) => {
            (ApiOffer? offer, int status) = await webapi.PostInquireGetOffer(inquire);
            if (offer != null && status >= 200 && status < 300) {
                offers.Add(offer);
            }
        });

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
            _inquireCodes.Add((codeList, inquireDB.Id));

            return Ok(offers.ToList());
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
                (int status, string? code) = await webapi.PostOrder(order);

                // retrieve cashed id
                int inquireId = _inquireCodes.FirstOrDefault(e => e.Item1.Contains(order.Code)).Item2;

                var inquireDB = _context.Inquires.FirstOrDefault(e => e.Id == inquireId);
                if (inquireDB == null) { return NotFound(); }
                if (code != null) {
                    inquireDB.Code = code;
                } else {
                    inquireDB.Code = order.Code;
                }

                if (status >= 200 && status < 300) {
                    Order orderDB = (Order)order;
                    orderDB.InquireId = inquireId;
                    orderDB.ServiceId = service.Id;
                    orderDB.StatusId = (int)StatusType.NotConfirmed;
                    await _context.Orders.AddAsync(orderDB);
                }
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
