using CourierHub.Server.Api;
using CourierHub.Server.Containers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Server.Data;
public class Repeater {
    private readonly IDictionary<string, IWebApi> _webApis;
    private readonly CourierHubDbContext _context;

    public Repeater(CourierHubDbContext context, WebApiContainer apiContainer) {
        _context = context;
        _webApis = new Dictionary<string, IWebApi>();
        foreach (var webapi in apiContainer.WebApis) {
            _webApis.Add(webapi.ServiceName, webapi);
        }
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
