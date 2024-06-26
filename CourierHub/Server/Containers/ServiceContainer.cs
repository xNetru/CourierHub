﻿using CourierHub.Shared.ApiModels;

namespace CourierHub.Server.Containers;
public class ServiceContainer {
    public IDictionary<string, ApiService> Services { get; } = new Dictionary<string, ApiService>();

    public ServiceContainer(IEnumerable<ApiService> services) {
        foreach (var service in services) {
            Services.Add(service.Name, service);
        }
    }
}
