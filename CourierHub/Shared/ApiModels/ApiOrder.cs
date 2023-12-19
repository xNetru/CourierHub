using CourierHub.Shared.Models;

namespace CourierHub.Shared.ApiModels;

public class ApiOrder {
    public string Code { get; set; } = null!;

    public decimal Price { get; set; }

    public string ClientEmail { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string ClientSurname { get; set; } = null!;

    public string ClientPhone { get; set; } = null!;

    public string ClientCompany { get; set; } = null!;

    public ApiAddress ClientAddress { get; set; } = null!;

    public static explicit operator ApiOrder(Order order) {
        return new ApiOrder {
            Code = order.Inquire.Code,
            Price = order.Price,
            ClientEmail = order.ClientEmail,
            ClientName = order.ClientName,
            ClientSurname = order.ClientSurname,
            ClientPhone = order.ClientPhone,
            ClientCompany = order.ClientCompany,
            ClientAddress = (ApiAddress)order.ClientAddress
        };
    }

    public static explicit operator Order(ApiOrder order) {
        return new Order {
            Price = order.Price,
            ClientEmail = order.ClientEmail,
            ClientName = order.ClientName,
            ClientSurname = order.ClientSurname,
            ClientPhone = order.ClientPhone,
            ClientCompany = order.ClientCompany,
            ClientAddress = (Address)order.ClientAddress
        };
    }
}
