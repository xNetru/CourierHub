using CourierHub.Server.Api;
using CourierHub.Server.Containers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Server.Data;
public class StatusUpdateService : BackgroundService {
    private readonly IDictionary<string, IWebApi> _webApis;
    private readonly CourierHubDbContext _context;
    private const int _minutes = 12 * 60;
    public StatusUpdateService() {
        _context = new CourierHubDbContext();
        _webApis = new Dictionary<string, IWebApi>();
        var apiContainer = new WebApiContainer(_context);
        foreach (var webapi in apiContainer.WebApis) {
            _webApis.Add(webapi.ServiceName, webapi);
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        try {
            while (!stoppingToken.IsCancellationRequested) {
                await UpdateOrderStatus();
                Thread.Sleep(TimeSpan.FromMinutes(_minutes));
            }
        } finally {
            await _context.DisposeAsync();
        }
    }
    private async Task UpdateOrderStatus() {
        // orders that were not fulfilled by service name
        var orders = await _context.Orders.Where(e => 
            e.Status.Id == 1 || e.Status.Id == 2 || e.Status.Id == 5
        ).GroupBy(e => e.Service.Name).ToListAsync();

        foreach (var group in orders) {
            IWebApi api = _webApis[group.Key];
            foreach (Order order in group.ToArray()) {
                var inquire = await _context.Inquires.FirstOrDefaultAsync(e => e.Id == order.InquireId);
                if (inquire == null) { continue; }
                (StatusType? status, int response, string? code) = await api.GetOrderStatus(inquire.Code);
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
