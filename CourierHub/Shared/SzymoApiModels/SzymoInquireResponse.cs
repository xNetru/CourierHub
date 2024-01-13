using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CourierHub.Shared.SzymoApiModels
{
    public record SzymoInquireResponse(
        string inquiryId,
        double totalPrice,
        string? currency,
        DateTime expiringAt,
        SzymoPriceBreakDown? priceBreakDown);
}
