﻿using Azure.Communication.Email;
using CourierHub.Cloud;
using CourierHub.Server.Data;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourierHub.Shared.Controllers;
[ApiController]
[Route("[controller]")]
public class ContentController : ControllerBase {
    private readonly ICloudStorage _storage;
    private readonly ICloudCommunicationService _communicationService;
    private readonly ContentCreator _creator;

    public ContentController(CourierHubDbContext context, IConfiguration config,
        ICloudStorage storage, ICloudCommunicationService communicationService) {
        _storage = storage;
        _communicationService = communicationService;
        string serviceName = config["ServiceName"] ??
            throw new NullReferenceException("Service name could not be loaded!");
        var service = (ApiService)context.Services.Where(s => s.Name == serviceName).FirstOrDefault()!;
        _creator = new ContentCreator(service);
    }

    // POST <ContentController>/contract/{...}
    [HttpPost("contract")]
    public async Task<ActionResult> PostContract([FromBody] ApiContract? contract) {
        if (contract == null) { return BadRequest(); }

        string content = _creator.CreateContract(contract);
        await _storage.PutBlobAsync($"{contract.Code}/contract_{contract.Code}.txt", "data", content, false);
        return Ok();
    }

    // POST <ContentController>/receipt/{...}
    [HttpPost("receipt")]
    public async Task<ActionResult> PostReceipt([FromBody] ApiReceipt? receipt) {
        if (receipt == null) { return BadRequest(); }

        string content = _creator.CreateReceipt(receipt);
        await _storage.PutBlobAsync($"{receipt.Code}/receipt_{receipt.Code}.txt", "data", content, false);
        return Ok();
    }

    // POST <ContentController>/mailcontent/{...}
    [HttpPost("mailcontent")]
    public async Task<ActionResult> PostMailContent([FromBody] ApiMailContent? content) {
        if (content == null) { return BadRequest(); }

        EmailContent body = _creator.CreateMailContent(content);

        var attachments = new List<EmailAttachment>();

        if (content.Contract != null && content.Status == Enums.StatusType.Confirmed) {
            string contract = _creator.CreateContract(content.Contract);
            EmailAttachment attach = _creator.CreateMailAttachment("contract.txt", contract);
            attachments.Add(attach);
        }

        if (content.Receipt != null && content.Status == Enums.StatusType.Confirmed) {
            string receipt = _creator.CreateReceipt(content.Receipt);
            EmailAttachment attach = _creator.CreateMailAttachment("receipt.txt", receipt);
            attachments.Add(attach);
        }

        var result = await _communicationService.SendEmailAsync(content.Recipient, body, attachments);
        if (result == null || result.Status == EmailSendStatus.Failed) {
            return StatusCode(503);
        }
        return Ok();
    }
}
