using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class WorkTypeStoreService : StoreService<WorkType, WorkTypeModel>, IWorkTypeStoreService
    {
        private DataContext _context;
        public WorkTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}