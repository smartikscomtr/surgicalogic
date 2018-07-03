using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class BranchStoreService : StoreService<Branch, BranchModel>, IBranchStoreService
    {
        public BranchStoreService(DataContext context) : base(context)
        {
        }
    }
}
