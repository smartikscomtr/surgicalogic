using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class OperationPlanStoreService : StoreService<OperationPlan, OperationPlanModel>, IOperationPlanStoreService
    {
        private DataContext _context;
        public OperationPlanStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}