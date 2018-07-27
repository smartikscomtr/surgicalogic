using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class OperationTypeEquipmentStoreService : StoreService<OperationTypeEquipment, OperationTypeEquipmentModel>, IOperationTypeEquipmentStoreService
    {
        private DataContext _context;
        public OperationTypeEquipmentStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}