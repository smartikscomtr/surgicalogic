using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentStoreService : StoreService<Equipment, EquipmentModel>, IEquipmentStoreService
    {
        private DataContext _context;
        public EquipmentStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}