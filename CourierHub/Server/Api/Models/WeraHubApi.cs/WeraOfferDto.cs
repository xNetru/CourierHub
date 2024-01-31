using CourierHub.Server.Api.Enums.WeraHubApi;

namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraOfferDto(
        string? companyOfferId,
        WeraInquiryDto inquiry,
        DateTime creationDate,
        DateTime expirationDate,
        double price,
        double taxes,
        double fees,
        WeraCurrency currency);
}
