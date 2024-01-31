using System.Net;
using System.Text.Json.Serialization;

namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraOfferResponse(
        WeraOfferDto result);
}
