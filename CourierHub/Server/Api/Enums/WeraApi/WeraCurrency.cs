using System.Text.Json.Serialization;

namespace CourierHub.Server.Api.Enums.WeraApi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WeraCurrency
    {
        PLN, EUR, USD, GBP
    }
}
