using CourierHub.Server.Api;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Server.Data;
public class Repeater {
    private readonly IDictionary<string, IWebApi> _webApis;
    private readonly CourierHubDbContext _context;

    public Repeater(CourierHubDbContext context, IConfiguration config) {
        _context = context;
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
        var webApis = new Dictionary<string, IWebApi> {
            { "CourierHub", new CourierHubApi(service) }
        };
        _webApis = webApis;
    }

    public async Task Repeat(int hours) {
        while (true) {
            await UpdateOrderStatus();
            await Task.Delay(TimeSpan.FromHours(hours).Milliseconds);
        }
    }

    private async Task UpdateOrderStatus() {
        // orders that were not fulfilled by service name
        var orders = await _context.Orders.Where(e => e.Status.Id < 6).GroupBy(e => e.Service.Name).ToListAsync();

        foreach (var group in orders) {
            IWebApi api = _webApis[group.Key];
            foreach (Order order in group.ToArray()) {
                (StatusType? status, int response, string? code) = await api.GetOrderStatus(order.Inquire.Code);
                if (status != null && response >= 200 && response < 300) {
                    order.StatusId = (int)status;
                    if (code != null) {
                        order.Inquire.Code = code;
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
