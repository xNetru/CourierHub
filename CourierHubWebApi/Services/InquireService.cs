using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using ErrorOr;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CourierHubWebApi.Services
{
    public class InquireService : IInquireService
    {
        private CourierHubDbContext _dbContext;
        public InquireService(CourierHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ErrorOr<Inquire>> CreateInquire(CreateInquireRequest request)
        {

            Address sourceAddress = request.CreateSourceAddress();
            Address destinationAddress = request.CreateDestinationAddress();
            Inquire inquire = request.CreateInquire();

            EntityEntry<Address> sourceAddressEntity = await _dbContext.Addresses.AddAsync(sourceAddress);
            EntityEntry<Address> destinationAddressEntity = await _dbContext.Addresses.AddAsync(destinationAddress);



            int writtenToDatabase = await _dbContext.SaveChangesAsync();

            PropertyValues? sourceAddressValues = await sourceAddressEntity.GetDatabaseValuesAsync();
            PropertyValues? destinationAddressValues = await destinationAddressEntity.GetDatabaseValuesAsync();
            if (sourceAddressValues == null || destinationAddressValues == null)
            {
                // TODO: rollback changes
                return Error.Failure();
            }

            int sourceAddressId, destinationAddressId;
            if (!sourceAddressValues.TryGetValue("Id", out sourceAddressId) ||
                !sourceAddressValues.TryGetValue("Id", out destinationAddressId))
            {
                // TODO: rollback changes
                return Error.Failure();
            }

            inquire.SourceId = sourceAddressId;
            inquire.DestinationId = destinationAddressId;

            EntityEntry<Inquire> inquireEntity = _dbContext.Inquires.Add(inquire);
            writtenToDatabase = await _dbContext.SaveChangesAsync();

            PropertyValues? inquirePropertyValues = await inquireEntity.GetDatabaseValuesAsync();

            if (inquirePropertyValues == null)
            {
                // TODO: rollback changes
                return Error.Failure();
            }

            int inquireId;
            if (!inquirePropertyValues.TryGetValue("Id", out inquireId))
            {
                // TODO: rollback changes
                return Error.Failure();
            }

            inquire.Id = inquireId;
            return inquire;
        }
    }
}
