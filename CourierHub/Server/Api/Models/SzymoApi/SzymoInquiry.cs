﻿namespace CourierHub.Api.Models.SzymoApi;

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
