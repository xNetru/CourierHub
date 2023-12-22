using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase {
        private readonly IEnumerable<IWebApi> _webApis;

        public ApiController(CourierHubDbContext context, IConfiguration config) {
            // w przyszłości z bazy danych, na czas testów z configa
            string adres = config.GetValue<string>("KamilBaseAddress") ??
                throw new NullReferenceException("Base address could not be loaded!");
            var service = new ApiService {
                Name = "CourierHub",
                ApiKey = "1fbbdd4f48fb4c87890cef420d865b86",
                BaseAddress = adres
            };
            var webApis = new List<IWebApi> {
                new CourierHubApi(service)
            };
            _webApis = webApis;
        }

        // POST: <ApiController>/inquire/{...}
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ApiOffer>>> PostInquireGetOffers([FromBody] ApiInquire? inquire) {
            if (inquire == null) { return BadRequest(Array.Empty<ApiOffer>()); }

            var offers = new List<ApiOffer>();
            foreach (var webapi in _webApis) {
                (ApiOffer? offer, int status) = await webapi.PostInquireGetOffer(inquire);
                if (offer != null && status >= 200 && status < 300) {
                    offers.Add(offer);
                }
            }
            if (offers.Any()) {
                return Ok(offers);
            } else {
                return NotFound(Array.Empty<ApiOffer>());
            }

        }

        // POST: <ApiController>/CourierHub/order/{...}
        [HttpPost("{serviceName}")]
        public async Task<ActionResult> PostOrder(string serviceName, [FromBody] ApiOrder? order) {
            if (order == null) { return BadRequest(); }

            foreach (var webapi in _webApis) {
                if (webapi.ServiceName == serviceName) {
                    int status = await webapi.PostOrder(order);
                    return StatusCode(status);
                }
            }
            return Ok();
        }

        // PATCH: <ApiController>/CourierHub/q1w2-e3r4-t5y6-u7i8-o9p0
        [HttpPost("{serviceName}/{code}")]
        public async Task<ActionResult> PutOrderWithrawal(string serviceName, string code) {
            throw new NotImplementedException();
        }
    }
}
