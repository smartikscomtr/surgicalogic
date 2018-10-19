using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperationPlanHistoryStoreService : StoreService<OperationPlan, OperationPlanHistoryModel>, IOperationPlanHistoryStoreService
    {
        private DataContext _context;
        public OperationPlanHistoryStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationListAsync(GridInputModel input)
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            var projectQuery = GetQueryable().Where(x => x.OperationDate > tomorrow && x.OperationDate < tomorrow.AddDays(1)).ProjectTo<OperationPlanHistoryOutputModel>();

            int totalCount = await projectQuery.CountAsync();

            if (input.PageSize > 0)
            {
                projectQuery = projectQuery.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);
            }

            var result = await projectQuery.ToListAsync();

            return new ResultModel<OperationPlanHistoryOutputModel>
            {
                Result = result,
                TotalCount = totalCount,
                Info = new Info()
            };
        }
    }
}