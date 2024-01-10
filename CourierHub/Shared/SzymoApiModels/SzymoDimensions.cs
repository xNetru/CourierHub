using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.SzymoApiModels
{
    public record SzymoDimensions(
        float width, 
        float height,
        float length,
        string dimensionUnit);
}
