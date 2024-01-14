using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using ErrorOr;
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
        public async Task<ErrorOr<CreateInquireResponse>> CreateInquire(CreateInquireRequest request, int serviceId) {
            Inquire inquire = request.CreateInquire();

            SetOrderCode(inquire);

            if (!_apiKeyService.IsOurServiceRequest(serviceId)) {
                Error? error = await AddInquireToDataBase(inquire);
                if (error != null) {
                    return error.Value;
                }
            }

            return CreateResponse(inquire);
        }
        public async Task<ErrorOr<CreateInquireResponse>> CreateInquireWithEmail(CreateInquireWithEmailRequest request, int serviceId) {
            Inquire inquire = request.CreateInquire();

            SetOrderCode(inquire);

            if (!TryGetUserId(request.Email, out var userId)) {
                return Error.NotFound();
            }

            inquire.ClientId = userId;

            Error? error = await AddInquireToDataBase(inquire);
            if (error != null) {
                return error.Value;
            }

            return CreateResponse(inquire);
        }
        private void SetOrderCode(Inquire inquire) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(DateTime.Now.Date.Year);
            stringBuilder.Append(DateTime.Now.Date.Month);
            stringBuilder.Append(DateTime.Now.Date.Day);
            stringBuilder.Append(DateTime.Now.Hour);
            stringBuilder.Append(DateTime.Now.Minute);
            stringBuilder.Append(DateTime.Now.Second);
            stringBuilder.Append(DateTime.Now.Millisecond);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
            inquire.Code = System.Convert.ToBase64String(plainTextBytes);
        }
        private decimal CalculatePrice(Inquire inquire) {

            Scaler? scaler = _dbContext.Scalers.Where(x => x.Id == 0).FirstOrDefault();
            if (scaler == null)
                return 0m;

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

            if(scaler.Fee != null)
                price *= (decimal)scaler.Fee;

            if (scaler.Tax != null)
                price *= (decimal)scaler.Tax;

            return price;
        }
        private async Task<Error?> AddInquireToDataBase(Inquire inquire) {
            try {
                await _dbContext.AddAsync(inquire);
                _dbContext.SaveChanges();
            } catch (Exception ex) {
                return Error.Failure();
            }
            return default;
        }
        private ErrorOr<CreateInquireResponse> CreateResponse(Inquire inquire) {
            decimal calculatedPrice = CalculatePrice(inquire);
            ErrorOr<DateTime> cacheResult = _priceCacheService.SavePrice(inquire.Code, calculatedPrice);

            DateTime? expirationTime = null;
            cacheResult.Match(time => { expirationTime = time; return 0; }, errors => { return 0; });
            if (expirationTime != null) {
                return new CreateInquireResponse(calculatedPrice, inquire.Code, DateTime.Now.AddMinutes(15));
            } else {
                return cacheResult.Errors;
            }
        }
        private bool TryGetUserId(string email, out int id) {
            id = 0;
            IQueryable<int> idQuery = from users
                                    in _dbContext.Users
                                      where users.Email == email &&
                                      users.Type == (int)UserType.Client
                                      select users.Id;
            if (!idQuery.Any())
                return false;
            try {
                id = idQuery.First();
                return true;
            } catch {
                return false;
            }
        }
    }
}
