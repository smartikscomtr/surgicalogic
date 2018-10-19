using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class SettingDataTypeStoreService : StoreService<SettingDataType, SettingDataTypeModel>, ISettingDataTypeStoreService
    {
        private DataContext _context;
        public SettingDataTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }        
    }
}