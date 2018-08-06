using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Surgicalogic.Services.Stores
{
    public class OperationStoreService : StoreService<Operation, OperationModel>, IOperationStoreService
    {
        private DataContext _context;
        public OperationStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Planning.Model.InputModel.OperationInputModel>> GetTomorrowOperationsAsync()
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            return await _context.Operations.Where(x => x.Date >= tomorrow && x.Date < tomorrow.AddDays(1) && x.IsActive).ProjectTo<Planning.Model.InputModel.OperationInputModel>().ToListAsync();
        }
    }
}
