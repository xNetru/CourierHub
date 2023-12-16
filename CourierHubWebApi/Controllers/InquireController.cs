using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourierHubWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquireController : ControllerBase
    {
        private IInquireService _inquireService;
        public InquireController(IInquireService inquireService)
        {
            _inquireService = inquireService;
        }   
        [HttpPost]
        public IActionResult CreateInquire(CreateInquireRequest request)
        {
            return _inquireService.CreateInquire(request).Result.Match(
                inquire => CreatedAtAction(
                    actionName: nameof(CreateInquire),
                    routeValues: new { id = inquire.Id },
                    value: new CreateInquireResponse(inquire.Id)),
                errors => Problem());
                
        }
    }
}
