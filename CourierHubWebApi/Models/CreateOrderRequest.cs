﻿namespace CourierHubWebApi.Models
{
    public record CreateOrderRequest(
        string InquireCode,
        string ClientName,
        string ClienSurname,
        string ClientEmail,
        string ClientPhoneNumber,
        string ClientCompany,
        string ClientCity,
        string ClientPostalCode,    
        string ClientStreet,
        string ClientAddressBuildingNumber,
        string? ClientFlat);
        
}
