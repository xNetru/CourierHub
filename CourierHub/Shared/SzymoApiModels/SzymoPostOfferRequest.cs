using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.SzymoApiModels
{
    public record SzymoPostOfferRequest(
        string inquiryId,
        string name,
        string email,
        SzymoAddress address);
}
