using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.SzymoApiModels
{
    public record SzymoGetOfferStatusResponse(
        string offerId,
        bool isReady,
        DateTime timestamp);

}
