using CourierHub.Server.Api.Enums.WeraApi;

namespace CourierHub.Server.Api.Models.WeraApi
{
    public record OfferDto(
        string? companyOfferId,
        WeraInquiryDto inquiry,
        DateTime creationDate,
        DateTime expirationDate,
        double price,
        double taxes,
        double fees,
        WeraCurrency currency);
}
