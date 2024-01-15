using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using CourierHubWebApi.Errors;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using ErrorOr;
using OneOf;

namespace CourierHubWebApi.Services {
    public class OrderService : IOrderService {
        private CourierHubDbContext _dbContext;
        private IPriceCacheService _priceCacheService;
        private IApiKeyService _apiKeyService;
        public OrderService(CourierHubDbContext dbContext, IPriceCacheService priceCacheService, IApiKeyService apiKeyService) {
            _dbContext = dbContext;
            _priceCacheService = priceCacheService;
            _apiKeyService = apiKeyService;
        }

        public OneOf<int, ApiError> CreateOrder(CreateOrderRequest request, int serviceId)
        {
            if (_apiKeyService.IsOurServiceRequest(serviceId))
                return StatusCodes.Status200OK;
            Order order = request.CreateOrder();
            Address clientAddress = request.CreateClientAddress();
            string inquiryCode = request.InquireCode;

            // checking whether offer is not expired
            OneOf<decimal,ApiError> result = _priceCacheService.GetPrice(inquiryCode, DateTime.Now);
            decimal? price = result.Match(x => x, x => default);
            if (price == default)
            {
                // TODO: return valid error
                return result.Match(x => ApiError.DefaultInternalServerError, x => x);
            }

            order.Price = (decimal)price;

            // taking matching inquiry from database 
            IQueryable<Inquire> inquiryIdQuery = from inquires
                                                 in _dbContext.Inquires
                                                 where inquires.Code == inquiryCode
                                                 select inquires;

            if (inquiryIdQuery.Count() != 1)
            {
                return ApiError.DefaultInternalServerError;
            }

            Inquire inquiry = inquiryIdQuery.First();

            order.StatusId = (int)StatusType.NotConfirmed;
            order.InquireId = inquiry.Id;
            order.ServiceId = serviceId;

            order.ClientAddress = clientAddress;

            try
            {
                _dbContext.Add(order);
                _dbContext.SaveChanges();
            }
            catch
            {
                return ApiError.DefaultInternalServerError;
            }
            return StatusCodes.Status200OK;
        }
        public async Task<OneOf<int, ApiError>> WithdrawOrder(WithdrawOrderRequest request, int serviceId)
        {
            IQueryable<Order> orders = _dbContext.Orders.Where(x => x.ServiceId == serviceId && x.Inquire.Code == request.Code);
            if (orders.Count() != 1)
            {
                // Spytać Bartka który kod
                if (orders.Count() == 0)
                    return new ApiError(StatusCodes.Status404NotFound, "No such order exists", "Order not found.");
                else
                    return ApiError.DefaultInternalServerError;
            }
            Order order = orders.First();
            Status? status = _dbContext.Statuses.Where(x => x.Id == order.StatusId).FirstOrDefault();
            if (status != null && status.IsCancelable)
            {
                try
                {
                    order.StatusId = (int)StatusType.Cancelled;
                    await _dbContext.SaveChangesAsync();
                    return StatusCodes.Status200OK;
                }
                catch
                {
                    return ApiError.DefaultInternalServerError;
                }
            }
            else
            {
                return new ApiError(StatusCodes.Status408RequestTimeout, "Withdrawal period elapsed.", "Order cannot be cancelled.");
            }

        }
        public OneOf<StatusType, ApiError> GetOrderStatus(GetOrderStatusRequest request, int serviceId)
        {
            IQueryable<Order> orders = _dbContext.Orders.Where(x => x.Inquire.Code == request.Code && x.ServiceId == serviceId);
            Order? order = orders.FirstOrDefault();
            if (orders.Count() != 1 || order == null)
            {
                if (order == null)
                    return new ApiError(StatusCodes.Status404NotFound, "No such order exists.", "Order not found.");
                return ApiError.DefaultInternalServerError;
            }
            return (StatusType)order.StatusId;
        }
    }
}
