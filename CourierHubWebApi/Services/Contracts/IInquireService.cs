﻿using CourierHub.Shared.Models;
using CourierHubWebApi.Models;
using ErrorOr;

namespace CourierHubWebApi.Services.Contracts
{
    public interface IInquireService
    {
        Task<ErrorOr<Inquire>> CreateInquire(CreateInquireRequest inquire);
    }
}
