namespace CourierHubWebApi.Models {
    /// <summary>
    /// Address 
    /// </summary>
    /// <param name="City" example="Lipinki Łużyckie"></param>
    /// <param name="PostalCode" example="01-016"></param>
    /// <param name="Street" example="Rozłączna"></param>
    /// <param name="Number" example="41"></param>
    /// <param name="Flat" example="2a"></param>
    public record ApiSideAddress(
        string City,
        string PostalCode,
        string Street,
        string Number,
        string? Flat);
}
