using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperationTypeStoreService : StoreService<OperationType, OperationTypeModel>, IOperationTypeStoreService
    {
        private DataContext _context;
        public OperationTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OperationTypeOutputModel>> GetByBranchIdAsync(int branchId)
        {
            return await GetQueryable().Where(x => x.BranchId == branchId).ProjectTo<OperationTypeOutputModel>().ToListAsync();
        }
    }
}