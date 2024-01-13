using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.SzymoApiModels
{
    public record SzymoInquiry(
        SzymoDimensions dimensions,
        string? currency,
        float weight,
        string? weightUnit,
        SzymoAddress source, 
        SzymoAddress destination,
        DateTime pickupDate,
        DateTime deliveryDay,
        bool deliveryInWeekend,
        string? priority,
        bool viaPackage,
        bool isCompany);
}
