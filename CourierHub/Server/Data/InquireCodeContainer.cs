﻿namespace CourierHub.Server.Data;
public class InquireCodeContainer {
    public List<(List<string>, int)> InquireCodes { get; set; } = new List<(List<string>, int)>();
}