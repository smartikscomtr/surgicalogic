using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomStoreService : StoreService<OperatingRoom, OperatingRoomModel>, IOperatingRoomStoreService
    {
        public OperatingRoomStoreService(DataContext context) : base(context)
        {
        }
    }
}