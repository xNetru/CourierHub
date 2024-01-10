using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.SzymoApiModels
{
    public record SzymoAddress(
        string? houseNumber,
        string? apartmentNumber,
        string? street,
        string? city,
        string? zipCode, 
        string? country);
}
