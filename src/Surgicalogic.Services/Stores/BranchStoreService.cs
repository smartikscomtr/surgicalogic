using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Linq;
using System.Threading.Tasks;

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