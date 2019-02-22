using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
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

        public async Task<List<OperationModel>> GetTomorrowOperationsAsync()
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            return await _context.Operations.Where(x => x.Date >= tomorrow && x.Date < tomorrow.AddDays(1) && x.IsActive).ProjectTo<OperationModel>().ToListAsync();
        }

        public async Task<List<OperationModel>> GetOperationsByDateAsync(DateTime operationDate)
        {
            return await _context.Operations.Where(x => x.Date >= operationDate && x.Date < operationDate.AddDays(1) && x.IsActive).ProjectTo<OperationModel>().ToListAsync();
        }

        public async Task<List<OperationModel>> GetByIdListAsync(int[] updatedItemIds)
        {
            return await GetQueryable().Where(x => updatedItemIds.Contains(x.Id)).ProjectTo<OperationModel>().ToListAsync();
        }
    }
}
