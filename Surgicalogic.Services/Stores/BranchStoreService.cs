using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class BranchStoreService : StoreService<Branch, BranchModel>, IBranchStoreService
    {
        private DataContext _context;
        public BranchStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}