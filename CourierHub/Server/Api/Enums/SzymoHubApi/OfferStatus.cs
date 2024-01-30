using System.Text.Json.Serialization;

namespace CourierHub.Server.Api.Enums.SzymoHubApi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OfferStatus
    {
        Accepted, Canceled
    }
}
