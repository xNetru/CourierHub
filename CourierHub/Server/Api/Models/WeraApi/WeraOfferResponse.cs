using System.Net;
using System.Text.Json.Serialization;

namespace CourierHub.Server.Api.Models.WeraApi
{
    public record WeraOfferResponse(
        WeraOfferDto result);
}
