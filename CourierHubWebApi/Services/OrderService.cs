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
                return result.Match(x => new ApiError(StatusCodes.Status500InternalServerError), x => x);
            }

            order.Price = (decimal)price;

            // taking matching inquiry from database 
            IQueryable<Inquire> inquiryIdQuery = from inquires
                                                 in _dbContext.Inquires
                                                 where inquires.Code == inquiryCode
                                                 select inquires;

            if (inquiryIdQuery.Count() != 1)
            {
                return new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error.");
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
                return new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error.");
            }
            return StatusCodes.Status200OK;
        }

        //public ErrorOr<int> CreateOrder(CreateOrderRequest request, int serviceId) {
        //    if (_apiKeyService.IsOurServiceRequest(serviceId))
        //        return StatusCodes.Status200OK;
        //    Order order = request.CreateOrder();
        //    Address clientAddress = request.CreateClientAddress();
        //    string inquiryCode = request.InquireCode;

        //    // checking whether offer is not expired
        //    ErrorOr<decimal> result = _priceCacheService.GetPrice(inquiryCode, DateTime.Now);
        //    decimal? price = result.Match(x => x, x => default);
        //    if (price == default) {
        //        // TODO: return valid error
        //        return Error.Failure();
        //    }

        //    order.Price = (decimal)price;

        //    // taking matching inquiry from database 
        //    IQueryable<Inquire> inquiryIdQuery = from inquires
        //                     in _dbContext.Inquires
        //                                         where inquires.Code == inquiryCode
        //                                         select inquires;

        //    if (inquiryIdQuery.Count() != 1) {
        //        // TODO: return valid error
        //        return Error.Failure();
        //    }

        //    Inquire inquiry = inquiryIdQuery.First();

        //    order.StatusId = (int)StatusType.NotConfirmed;
        //    order.InquireId = inquiry.Id;
        //    order.ServiceId = serviceId;

        //    order.ClientAddress = clientAddress;

        //    try {
        //        _dbContext.Add(order);
        //        _dbContext.SaveChanges();
        //    } catch {
        //        // TODO: rollback changes
        //        return Error.Failure();
        //    }
        //    return StatusCodes.Status200OK;
        //}
        public async Task<OneOf<int, ApiError>> WithdrawOrder(WithdrawOrderRequest request, int serviceId)
        {
            IQueryable<Order> orders = _dbContext.Orders.Where(x => x.ServiceId == serviceId && x.Inquire.Code == request.Code);
            if (orders.Count() != 1)
            {
                // Spytać Bartka który kod
                if (orders.Count() == 0)
                    return new ApiError(StatusCodes.Status404NotFound, "No such order exists", "Order not found.");
                else
                    return new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error.");
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
                    return new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error.");
                }
            }
            else
            {
                return new ApiError(StatusCodes.Status408RequestTimeout, "Withdrawal period elapsed.", "Order cannot be cancelled.");
            }

        }
        //public async Task<ErrorOr<int>> WithdrawOrder(WithdrawOrderRequest request, int serviceId) {
        //    IQueryable<Order> orders = _dbContext.Orders.Where(x => x.ServiceId == serviceId && x.Inquire.Code == request.Code);
        //    if (orders.Count() != 1) {
        //        // TODO: pass valid error
        //        return Error.Failure();
        //    }
        //    Order order = orders.First();
        //    Status? status = _dbContext.Statuses.Where(x => x.Id == order.StatusId).FirstOrDefault();
        //    if (status != null && status.IsCancelable) {
        //        try {
        //            order.StatusId = (int)StatusType.Cancelled;
        //            await _dbContext.SaveChangesAsync();
        //            return StatusCodes.Status200OK;
        //        } catch (Exception ex) {
        //            // TODO: pass valid error
        //            return Error.Failure();
        //        }
        //    } else {
        //        // TODO: pass valid error
        //        return Error.Failure();
        //    }

        //}

        public OneOf<StatusType, ApiError> GetOrderStatus(GetOrderStatusRequest request, int serviceId)
        {
            IQueryable<Order> orders = _dbContext.Orders.Where(x => x.Inquire.Code == request.Code && x.ServiceId == serviceId);
            Order? order = orders.FirstOrDefault();
            if (orders.Count() != 1 || order == null)
            {
                return new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error.");
            }
            return (StatusType)order.StatusId;
        }
        //public ErrorOr<StatusType> GetOrderStatus(GetOrderStatusRequest request, int serviceId) {
        //    IQueryable<Order> orders = _dbContext.Orders.Where(x => x.Inquire.Code == request.Code && x.ServiceId == serviceId);
        //    Order? order = orders.FirstOrDefault();
        //    if (orders.Count() != 1 || order == null) {
        //        return Error.Conflict();
        //    }
        //    return (StatusType)order.StatusId;
        //}
    }
}
