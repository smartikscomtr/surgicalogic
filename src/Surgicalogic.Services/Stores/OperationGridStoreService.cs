using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class OperationGridStoreService : StoreService<Operation, OperationGridModel>, IOperationGridStoreService
    {
        private DataContext _context;
        public OperationGridStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
