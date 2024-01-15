using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.DatabaseTest; 
public class DatabaseTest : IClassFixture<CourierHubDbContext> {

    private readonly CourierHubDbContext _context;
    private readonly string _serviceName = "CourierHub";

    public DatabaseTest(CourierHubDbContext context) {
        _context = context;
    }

    [Fact]
    public async Task ClientLogsIn() {
        using var transaction = _context.Database.BeginTransaction();
        // Given a client
        var adr1 = new Address() {
            Street = "Wiejska",
            Number = "4",
            PostalCode = "00-902",
            City = "Warszawa"
        };
        var adr2 = new Address() {
            Street = "Wolanowska",
            Number = "120",
            PostalCode = "26-600",
            City = "Radom"
        };
        var client = new User() {
            Name = "Mariusz",
            Surname = "Kamiński",
            Email = "mariuszkamiński@gmail.com",
            Type = 0
        };
        var data = new ClientData() {
            Company = "Areszt Śledczy Radom",
            SourceAddress = adr1,
            Address = adr2,
        };

        // When he tries to log in
        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == client.Email);

        // Then if he is not registered, he is added
        if (user == null) {
            await _context.Users.AddAsync(client);
            await _context.SaveChangesAsync();

            user = await _context.Users.FirstOrDefaultAsync(e => e.Email == client.Email);
            Assert.NotNull(user);

            data.ClientId = user.Id;
            await _context.ClientDatum.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // And receives his information back
        var dataDB = await _context.ClientDatum.FirstOrDefaultAsync(e => e.ClientId == user.Id);
        Assert.NotNull(dataDB);
        Assert.Equal(data.Company, dataDB.Company);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task OfficeWorkerViewsAndAcceptsOrders() {
        using var transaction = _context.Database.BeginTransaction();
        // Given a worker
        var worker = new User() {
            Name = "Janusz",
            Surname = "Kowalski",
            Email = "januszkowalski@gmail.com",
            Type = 1
        };

        // When he selects not-confirmed order
        var orders = await _context.Orders.Where(e =>
            e.Service.Name == _serviceName &&
            e.StatusId == 1).ToListAsync();

        if (orders.Count == 0) { return; }
        var order = orders.First();

        // And changes the status
        order.StatusId = 2;
        await _context.SaveChangesAsync();

        // Then the order's status changes
        var orderDB = await _context.Orders.FirstOrDefaultAsync(e => e.Id == order.Id);
        Assert.NotNull(orderDB);
        Assert.Equal(2, orderDB.StatusId);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CourierPicksUpAnOrder() {
        using var transaction = _context.Database.BeginTransaction();
        // Given a courier
        var courier = await _context.Users.FirstOrDefaultAsync(e => e.Type == 2);

        if (courier == null) {
            var courierDB = new User() {
                Name = "Janusz",
                Surname = "Kowalski",
                Email = "januszkowalski@gmail.com",
                Type = 2
            };

            await _context.Users.AddAsync(courierDB);
            await _context.SaveChangesAsync();

            courier = await _context.Users.FirstOrDefaultAsync(e => e.Email == courierDB.Email);
            Assert.NotNull(courier);
        }

        // When he selects confirmed order
        var orders = await _context.Orders.Where(e =>
            e.Service.Name == _serviceName &&
            e.StatusId == 2).ToListAsync();

        if (orders.Count == 0) { return; }
        var order = orders.First();

        // And changes the status
        order.StatusId = 5;
        await _context.SaveChangesAsync();

        // And creates a parcel
        var parcel = new Parcel() {
            CourierId = courier.Id,
            PickupDatetime = DateTime.Now
        };

        // Then the order's status changes
        var orderDB = await _context.Orders.FirstOrDefaultAsync(e => e.Id == order.Id);
        Assert.NotNull(orderDB);
        Assert.Equal(5, orderDB.StatusId);

        // And parcel appears
        parcel = await _context.Orders.Select(e => e.Parcel).Where(e =>
            e != null &&
            e.CourierId == parcel.CourierId &&
            e.PickupDatetime == parcel.PickupDatetime).FirstAsync();

        Assert.NotNull(parcel);
        Assert.Equal(courier.Id, parcel.CourierId);

        await transaction.RollbackAsync();
    }
}