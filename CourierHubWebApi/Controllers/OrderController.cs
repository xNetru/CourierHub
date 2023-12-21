using CourierHubWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourierHubWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost]

        CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
