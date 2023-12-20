using Azure.Communication.Email;
using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase {
        private readonly ICloudStorage _storage;
        private readonly ICloudCommunicationService _communicationService;
        private readonly ContentCreator _creator;

        public ContentController(CourierHubDbContext context, IConfiguration config, ICloudStorage storage, ICloudCommunicationService communicationService) {
            _storage = storage;
            _communicationService = communicationService;
            string serviceName = config.GetValue<string>("ServiceName") ??
                throw new NullReferenceException("Service name could not be loaded!");
            var service = (ApiService)context.Services.Where(s => s.Name == serviceName).FirstOrDefault()!;
            _creator = new ContentCreator(service);
        }

        // POST <ContentController>/contract/{...}
        [HttpPost]
        public async Task<ActionResult> PostContract([FromBody] ApiContract? contract) {
            if (contract == null) { return BadRequest(); }

            string content = _creator.CreateContract(contract);
            // code musi być z ApiContract
            await _storage.PutBlobAsync($"{contract.Code}/contract_{contract.Code}.txt", "data", content, false);
            return Ok();
        }

        // POST <ContentController>/receipt/{...}
        [HttpPost]
        public async Task<ActionResult> PostReceipt([FromBody] ApiReceipt? receipt) {
            if (receipt == null) { return BadRequest(); }

            string content = _creator.CreateReceipt(receipt);
            // code musi być z ApiReceipt
            await _storage.PutBlobAsync($"{receipt.Code}/receipt_{receipt.Code}.txt", "data", content, false);
            return Ok();
        }

        // POST <ContentController>/mailcontent/{...}
        [HttpPost]
        public async Task<ActionResult> PostMailContent([FromBody] ApiMailContent? content) {
            if (content == null) { return BadRequest(); }

            EmailContent body = _creator.CreateMailContent(content);
            var result = await _communicationService.SendEmailAsync(content.Recipient, body);
            if (result == null || result.Status == EmailSendStatus.Failed) {
                return StatusCode(503);
            }
            return Ok();
        }
    }
}
