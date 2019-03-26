using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class SettingValueStoreService : StoreService<SettingValue, SettingValueModel>, ISettingValueStoreService
    {
        private DataContext _context;
        public SettingValueStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
