using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.ExportModel.Report;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperationPlanHistoryStoreService : IOperationPlanHistoryStoreService
    {
        private DataContext _context;
        public OperationPlanHistoryStoreService(DataContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationListAsync(GridInputModel input)
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            var projectQuery = _context.OperationPlans.Where(x => x.IsActive && x.OperationDate > tomorrow && x.OperationDate < tomorrow.AddDays(1)).OrderBy(x => x.OperationDate).ProjectTo<OperationPlanHistoryOutputModel>();

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

        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetOperationListByDate(GridInputModel input, DateTime operationDate)
        {
            var projectQuery = _context.OperationPlans.Where(x => x.IsActive && x.OperationDate > operationDate && x.OperationDate < operationDate.AddDays(1)).OrderByDescending(x => x.OperationDate).ProjectTo<OperationPlanHistoryOutputModel>();

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

        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetAsync<TOutputModel>(HistoryPlanningInputModel input)
        {
            var query = _context.OperationPlans.Where(x => x.IsActive);

            var operatingRoomIds = input.OperatingRoomId?.Split(',').Select(int.Parse).ToList();
            var operationIds = input.OperationId?.Split(',').Select(int.Parse).ToList();

            if (!string.IsNullOrEmpty(input.SortBy))
            {
                Expression<Func<OperationPlan, object>> orderBy = null;

                switch (input.SortBy)
                {
                    case "operationName":
                        orderBy = x => x.Operation.Name;
                        break;
                    case "operationRoomName":
                        orderBy = x => x.OperatingRoom.Name;
                        break;
                    case "operationDate":
                        orderBy = x => x.OperationDate;
                        break;
                    case "identityNumber":
                        orderBy = x => x.Operation.Patient.IdentityNumber;
                        break;
                    default:
                        orderBy = x => x.Id;
                        break;
                }

                if (input.Descending == true)
                {
                    query = query.OrderByDescending(orderBy);
                }

                else
                {
                    query = query.OrderBy(orderBy);
                }
            }

            if (operatingRoomIds?.Count > 0)
            {
                query = query.Where(x => operatingRoomIds.Contains(x.OperatingRoomId));
            }

            if (operationIds?.Count > 0)
            {
                query = query.Where(x => operationIds.Contains(x.OperationId));
            }

            if (input.OperationStartDate != null && input.OperationStartDate != DateTime.MinValue)
            {
                query = query.Where(x => x.OperationDate >= input.OperationStartDate);
            }

            if (input.OperationEndDate !=null && input.OperationEndDate != DateTime.MinValue)
            {
                query = query.Where(x => x.OperationDate <= input.OperationEndDate.AddDays(1));
            }

            if (!string.IsNullOrEmpty(input.IdentityNumber))
            {
                query = query.Where(x => x.Operation.Patient.IdentityNumber == input.IdentityNumber);
            }

            if (!string.IsNullOrEmpty(input.Search))
            {
                query = query.Where(x =>
                    x.Operation.Name.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    x.OperatingRoom.Name.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    x.Operation.Patient.IdentityNumber.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0
                );
            }

            int totalCount = await query.CountAsync();

            if (input.PageSize > 0)
            {
                query = query.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);
            }

            var result = await query.ProjectTo<OperationPlanHistoryModel>().ToListAsync();

            return new ResultModel<OperationPlanHistoryOutputModel>
            {
                Result = AutoMapper.Mapper.Map<List<OperationPlanHistoryOutputModel>>(result),
                TotalCount = totalCount,
                Info = new Info { Succeeded = true }
            };

            //return base.GetAsync<TOutputModel>(input, expression);
        }

        public async Task<List<HistoryPlanningReportExportModel>> GetExportAsync<OperationPlanHistoryOutputModel>(HistoryPlanningInputModel input)
        {
            var query = _context.OperationPlans.Where(x => Convert.ToInt32((x.RealizedEndDate - x.RealizedStartDate).TotalMinutes) != x.Operation.OperationTime);
            
            return await query.ProjectTo<HistoryPlanningReportExportModel>().ToListAsync();
        }
    }
}