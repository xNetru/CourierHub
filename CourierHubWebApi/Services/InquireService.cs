using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using ErrorOr;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;

namespace CourierHubWebApi.Services {
    public class InquireService : IInquireService {
        private CourierHubDbContext _dbContext;
        private IPriceCacheService _priceCacheService; // Must be changed if only database is updated
        public InquireService(CourierHubDbContext dbContext, IPriceCacheService priceCacheService) {
            _dbContext = dbContext;
            _priceCacheService = priceCacheService;
        }
        public async Task<ErrorOr<CreateInquireResponse>> CreateInquire(CreateInquireRequest request, int serviceId) {

            if (serviceId == 1) // change it checking in database
            {
                return CreateInquireForHub(request);
            } else {
                return await CreateInquireForOther(request);
            }

        }
        private async Task<ErrorOr<CreateInquireResponse>> CreateInquireForOther(CreateInquireRequest request) {
            Address sourceAddress = request.CreateSourceAddress();
            Address destinationAddress = request.CreateDestinationAddress();
            Inquire inquire = request.CreateInquire();

            EntityEntry<Address> sourceAddressEntity;
            EntityEntry<Address> destinationAddressEntity;
            try {
                sourceAddressEntity = await _dbContext.Addresses.AddAsync(sourceAddress);
                destinationAddressEntity = await _dbContext.Addresses.AddAsync(destinationAddress);
            } catch (Exception ex) {
                return Error.Failure();
            }

            int writtenToDatabase;
            try {
                writtenToDatabase = await _dbContext.SaveChangesAsync();
            } catch (Exception ex) {
                // TODO: rollback changes
                return Error.Failure();
            }

            PropertyValues? sourceAddressValues = await sourceAddressEntity.GetDatabaseValuesAsync();
            PropertyValues? destinationAddressValues = await destinationAddressEntity.GetDatabaseValuesAsync();
            if (sourceAddressValues == null || destinationAddressValues == null) {
                // TODO: rollback changes
                return Error.Failure();
            }

            int sourceAddressId, destinationAddressId;
            if (!sourceAddressValues.TryGetValue("Id", out sourceAddressId) ||
                !sourceAddressValues.TryGetValue("Id", out destinationAddressId)) {
                // TODO: rollback changes
                return Error.Failure();
            }

            inquire.SourceId = sourceAddressId;
            inquire.DestinationId = destinationAddressId;
            SetOrderCode(inquire);

            EntityEntry<Inquire> inquireEntity = _dbContext.Inquires.Add(inquire);
            try {
                writtenToDatabase = await _dbContext.SaveChangesAsync();
            } catch (Exception ex) {
                // TODO: rollback changes
                return Error.Failure();
            }

            PropertyValues? inquirePropertyValues = await inquireEntity.GetDatabaseValuesAsync();

            if (inquirePropertyValues == null) {
                // TODO: rollback changes
                return Error.Failure();
            }

            int inquireId;
            if (!inquirePropertyValues.TryGetValue("Id", out inquireId)) {
                // TODO: rollback changes
                return Error.Failure();
            }

            decimal calculatedPrice = CalculatePrice(inquire);
            ErrorOr<DateTime> cacheResult = _priceCacheService.SavePrice(inquire.Code, calculatedPrice);

            DateTime? expirationTime = null;
            cacheResult.Match(time => { expirationTime = time; return 0; }, errors => { return 0; });
            if (expirationTime != null) {
                return new CreateInquireResponse(CalculatePrice(inquire), inquire.Code, DateTime.Now.AddMinutes(15));
            } else {
                return cacheResult.Errors;
            }
        }
        private ErrorOr<CreateInquireResponse> CreateInquireForHub(CreateInquireRequest request) {
            Inquire inquire = request.CreateInquire();
            SetOrderCode(inquire);
            decimal calculatedPrice = CalculatePrice(inquire);
            ErrorOr<DateTime> cacheResult = _priceCacheService.SavePrice(inquire.Code, calculatedPrice);

            DateTime? expirationTime = null;
            cacheResult.Match(time => { expirationTime = time; return 0; }, errors => { return 0; });
            if (expirationTime != null) {
                return new CreateInquireResponse(CalculatePrice(inquire), inquire.Code, DateTime.Now.AddMinutes(15));
            } else {
                return cacheResult.Errors;
            }
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
            // TODO: implement price calculation
            return 0m;
        }
    }
}
