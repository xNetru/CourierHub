using System.Text.Json.Serialization;

namespace CourierHub.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PriorityType {
    Low,
    Medium,
    High
}

public enum PriorityTypePL {
    Niski,
    Średni,
    Wysoki
}