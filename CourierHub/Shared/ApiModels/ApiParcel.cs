using CourierHub.Shared.Models;

namespace CourierHub.Shared.ApiModels;

public class ApiParcel {
    public DateTime? PickupDatetime { get; set; }

    public DateTime? DeliveryDatetime { get; set; }

    public string? UndeliveredReason { get; set; }

    public static explicit operator Parcel(ApiParcel parcel) {
        return new Parcel {
            PickupDatetime = parcel.PickupDatetime,
            DeliveryDatetime = parcel.DeliveryDatetime,
            UndeliveredReason = parcel.UndeliveredReason
        };
    }
}
