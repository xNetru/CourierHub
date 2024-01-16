using System.Text.Json.Serialization;

namespace CourierHub.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusType {
    NotConfirmed = 1,
    Confirmed = 2,
    Cancelled = 3,
    Denied = 4,
    PickedUp = 5,
    Delivered = 6,
    CouldNotDeliver = 7
}

public enum StatusTypePL {
    Niepotwierdzone = 1,
    Potwierdzone = 2,
    Anulowane = 3,
    Odrzucone = 4,
    Odebrane = 5,
    Dostarczone = 6,
    Nieudana_dostawa = 7
}