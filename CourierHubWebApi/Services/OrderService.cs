using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
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
        public ErrorOr<int> CreateOrder(CreateOrderRequest request, int serviceId)
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

            // taking matching inquiry from database 
            IQueryable<Inquire> inquiryIdQuery = from inquires
                             in _dbContext.Inquires
                             where inquires.Code == inquiryCode
                             select inquires;

            if(inquiryIdQuery.Count() != 1)
            {
                // TODO: return valid error
                return Error.Failure();
            }

            Inquire inquiry = inquiryIdQuery.First();

            // in case of request from our hub checking whether client is registerded
            //if(serviceId == 1)
            //{
            //    IQueryable<int> clientIdQuery = from users
            //                                    in _dbContext.Users
            //                                    where users.Email == order.ClientEmail &&
            //                                    users.Type == (int)UserType.Client
            //                                    select users.Id;

            //    if (clientIdQuery.Count() == 1)
            //    {
            //        int clientId = clientIdQuery.First();
            //        inquiry.ClientId = clientId;
            //    }
            //}

            order.InquireId = inquiry.Id;
            order.ServiceId = serviceId;

            order.ClientAddress = clientAddress;

            try
            {
                _dbContext.Add(order);
            }
            catch(Exception ex)
            {
                // TODO: rollback changes
                return Error.Failure();
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                // TODO: rollback changes
                return Error.Failure();
            }
            return StatusCodes.Status200OK;
        }
    }
}
