using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using ErrorOr;

namespace CourierHubWebApi.Services
{
    public class OrderService : IOrderService
    {
        private CourierHubDbContext _dbContext;
        private IPriceCacheService _priceCacheService;
        public OrderService(CourierHubDbContext dbContext, IPriceCacheService priceCacheService)
        {
            _dbContext = dbContext;
            _priceCacheService = priceCacheService;
        }
        public ErrorOr<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
        {
            Order order = request.CreateOrder();
            Address clientAddress = request.CreateClientAddress();
            string inquiryCode = request.InquireCode;

            // checking whether offer is not expired

            ErrorOr<decimal> result = _priceCacheService.GetPrice(inquiryCode, DateTime.Now);
            decimal? price = result.Match(x => x, x => default);
            if(price == default)
            {
                // TODO: return valid error
                return Error.Failure();
            }

            order.Price = (decimal)price;

            int? Cli

            try
            {
                _dbContext.Addresses.Add(clientAddress);
            }
            catch (Exception ex) 
            {
                return Error.Failure();
            }

            
        }
    }
}
