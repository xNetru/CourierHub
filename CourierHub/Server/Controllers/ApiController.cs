using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CourierHub.Shared.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase {
        private readonly IEnumerable<IWebApi> _webApis;

        public ApiController(CourierHubDbContext context, IConfiguration config) {
            // w przyszłości z bazy danych, na czas testów z configa
            // -----
            string adres = config.GetValue<string>("ApiAddress") ??
                throw new NullReferenceException("Base address could not be loaded!");
            var service = new ApiService {
                Name = "CourierHub",
                ApiKey = "1fbbdd4f48fb4c87890cef420d865b86",
                BaseAddress = adres
            };
            // -----
            var webApis = new List<IWebApi> {
                new CourierHubApi(service)
            };
            _webApis = webApis;
        }

        // POST: <ApiController>/inquire/{...}
        [HttpPost("inquire")]
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
        [HttpPost("{serviceName}/order")]
        public async Task<ActionResult> PostOrder(string serviceName, [FromBody] ApiOrder? order) {
            if (order == null) { return BadRequest(); }

            foreach (var webapi in _webApis) {
                if (webapi.ServiceName == serviceName) {
                    int status = await webapi.PostOrder(order);
                    return StatusCode(status);
                }
            }
            return NotFound(); // should not happen if serviceName exists
        }

        // PATCH: <ApiController>/CourierHub/cancel/q1w2-e3r4-t5y6-u7i8-o9p0
        [HttpPatch("{serviceName}/cancel/{code}")]
        public async Task<ActionResult> PutOrderWithrawal(string serviceName, string code) {
            foreach (var webapi in _webApis) {
                if (webapi.ServiceName == serviceName) {
                    int status = await webapi.PutOrderWithrawal(code);
                    return StatusCode(status);
                }
            }
            return NotFound(); // should not happen if serviceName exists
        }

        // to jest chyba realnie niepotrzebne, bo nie będzie wołane z frontendu
        // GET: <ApiController>/CourierHub/status/q1w2-e3r4-t5y6-u7i8-o9p0
        [HttpGet("{serviceName}/status/{code}")]
        public async Task<ActionResult<StatusType?>> GetOrderStatus(string serviceName, string code) {
            foreach (var webapi in _webApis) {
                if (webapi.ServiceName == serviceName) {
                    (StatusType? type, int status) = await webapi.GetOrderStatus(code);
                    if (type != null && status >= 200 && status < 300) {
                        return Ok(type);
                    } else {
                        return BadRequest(type);
                    }
                }
            }
            return NotFound(null); // should not happen if serviceName exists
        }
    }
}
