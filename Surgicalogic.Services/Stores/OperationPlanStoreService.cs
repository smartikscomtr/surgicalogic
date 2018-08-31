using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
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

        public async Task DeleteTomorrowPlanAsync()
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            var twoDaysLater = tomorrow.AddDays(1);

            var items = await GetQueryable().Where(x => x.OperationDate > tomorrow && x.OperationDate < twoDaysLater).ToListAsync();

            foreach (var item in items)
            {
                await DeleteByIdAsync(item.Id);
            }

            await SaveChangesAsync();
        }

        public async Task<List<OperationPlanOutputModel>> GetTomorrowOperationsAsync()
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            return await GetQueryable().Where(x => x.OperationDate > tomorrow && x.OperationDate < tomorrow.AddDays(1)).ProjectTo<OperationPlanOutputModel>().ToListAsync();
        }
    }
}