using AngleSharp.Css.Dom;

namespace CourierHub.Server.Api.Models.WeraApi
{
    public record WeraPackageDto(
        double width,
        double height, 
        double length,
        string dimensionsUnit,
        double weight,
        string weightUnit);
        
}
