using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Errors;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using OneOf;
using System.Text;

namespace CourierHubWebApi.Services {
    public class InquireService : IInquireService {
        private CourierHubDbContext _dbContext;
        private IPriceCacheService _priceCacheService; // Must be changed if only database is updated
        private IApiKeyService _apiKeyService;
        public InquireService(CourierHubDbContext dbContext, IPriceCacheService priceCacheService, IApiKeyService apiKeyService) {
            _dbContext = dbContext;
            _priceCacheService = priceCacheService;
            _apiKeyService = apiKeyService;
        }

        public async Task<OneOf<CreateInquireResponse, ApiError>> CreateInquire(CreateInquireRequest request, int serviceId) {
            Inquire inquire = request.CreateInquire();

            if (!IsValidServiceId(serviceId))
                return ApiError.DefaultInternalServerError;

            SetOrderCode(inquire);

            if (!_apiKeyService.IsOurServiceRequest(serviceId)) {
                int statusCode = await AddInquireToDataBase(inquire);
                if (statusCode != StatusCodes.Status200OK) {
                    return new ApiError(statusCode, null, "Internal server error");
                }
            }

            return CreateResponse(inquire);
        }
        private void SetOrderCode(Inquire inquire) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(inquire.Datetime.Date.Year);
            stringBuilder.Append(inquire.Destination.City.Last());
            stringBuilder.Append(inquire.Datetime.Date.Month);
            stringBuilder.Append(inquire.Source.City.Last());
            stringBuilder.Append(inquire.Source.PostalCode.First());
            stringBuilder.Append(inquire.Datetime.Date.Day);
            stringBuilder.Append(inquire.Datetime.Hour);
            stringBuilder.Append(inquire.Datetime.Minute);
            stringBuilder.Append(inquire.Destination.PostalCode.Last());
            stringBuilder.Append(inquire.Datetime.Second);
            stringBuilder.Append(inquire.Datetime.Millisecond);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
            inquire.Code = System.Convert.ToBase64String(plainTextBytes);
        }
        private decimal CalculatePrice(Inquire inquire) {

            Scaler? scaler = _dbContext.Scalers.FirstOrDefault();
            if (scaler == null)
                return decimal.Round(0m, 2);

            decimal price = (scaler.Width == null ? 1m : (decimal)scaler.Width) * inquire.Width +
                (scaler.Depth == null ? 1m : (decimal)scaler.Depth) * inquire.Depth +
                (scaler.Length == null ? 1m : (decimal)scaler.Length) * inquire.Length +
                (scaler.Mass == null ? 1m : (decimal)scaler.Mass) * inquire.Mass;

            if (inquire.IsCompany && scaler.Company != null)
                price *= (decimal)scaler.Company;

            if (inquire.IsWeekend && scaler.Weekend != null)
                price *= (decimal)scaler.Weekend;

            if (scaler.Priority != null)
                price += inquire.Priority * (decimal)scaler.Priority;

            if (scaler.Fee != null)
                price *= (decimal)scaler.Fee;

            if (scaler.Tax != null)
                price *= (decimal)scaler.Tax;

            
            return decimal.Round(price, 2);
        }

        private async Task<int> AddInquireToDataBase(Inquire inquire) {
            try {
                await _dbContext.AddAsync(inquire);
                _dbContext.SaveChanges();
            } catch {
                return StatusCodes.Status500InternalServerError;
            }
            return StatusCodes.Status200OK;
        }
        private OneOf<CreateInquireResponse, ApiError> CreateResponse(Inquire inquire) {
            decimal calculatedPrice = CalculatePrice(inquire);
            OneOf<DateTime, ApiError> cacheResult = _priceCacheService.SavePrice(inquire.Code, calculatedPrice);

            return cacheResult.Match(time => (OneOf<CreateInquireResponse, ApiError>)new CreateInquireResponse(calculatedPrice, inquire.Code, time), statusCode => statusCode);
        }
        private bool IsValidServiceId(int serviceId) {
            return _dbContext.Services.Where(x => x.Id == serviceId).Any();
        }
    }
}
