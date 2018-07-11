using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentTypeStoreService : StoreService<EquipmentType, EquipmentTypeModel>, IEquipmentTypeStoreService
    {
        private DataContext _context;
        public EquipmentTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}