﻿namespace CourierHub.Api.Models.SzymoHubApi;

public record SzymoInquireResponse(
    string inquiryId,
    double totalPrice,
    string? currency,
    DateTime expiringAt);//,
                         //SzymoPriceBreakDown? priceBreakDown);
