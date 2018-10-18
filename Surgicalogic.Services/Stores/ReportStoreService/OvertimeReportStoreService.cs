using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores.IReportStoreService;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores.ReportStoreService
{
    public class OvertimeReportStoreService : IOvertimeReportStoreService
    {
        DataContext _dataContext;
        public OvertimeReportStoreService(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<ResultModel<OvertimeReportOutputModel>> GetAsync<TOutputModel>(OvertimeReportInputModel input)
        {
            var query = _dataContext.OperationPlans.Where(x => Convert.ToInt32((x.RealizedEndDate - x.RealizedStartDate).TotalMinutes) != x.Operation.OperationTime);

            if (!string.IsNullOrEmpty(input.SortBy))
            {
                Expression<Func<OperationPlan, object>> orderBy = null;

                switch (input.SortBy)
                {
                    case "branchName":
                        orderBy = x => x.Operation.OperationType.Branch.Name;
                        break;
                    case "operationName":
                        orderBy = x => x.Operation.Name;
                        break;
                    case "operationRoomName":
                        orderBy = x => x.OperatingRoom.Name;
                        break;
                    case "operationStartDate":
                        orderBy = x => x.OperationDate;
                        break;
                    case "operationEndDate":
                        orderBy = x => x.OperationDate.AddMinutes(x.Operation.OperationTime);
                        break;
                    case "realizedStartDate":
                        orderBy = x => x.RealizedStartDate;
                        break;
                    case "realizedEndDate":
                        orderBy = x => x.RealizedEndDate;
                        break;
                    case "operationTimeDifference":
                        orderBy = x => x.Operation.OperationTime - Convert.ToInt32((x.RealizedEndDate - x.RealizedStartDate).TotalMinutes);
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

            if (input.BranchId > 0)
            {
                query = query.Where(x => x.Operation.OperationType.BranchId == input.BranchId);
            }

            if (input.DoctorId > 0)
            {
                query = query.Where(x => x.Operation.OperationPersonels.Any(t => t.PersonnelId == input.DoctorId));
            }

            if (input.RealizedStartDate != null && input.RealizedStartDate != DateTime.MinValue)
            {
                query = query.Where(x => x.RealizedStartDate >= input.RealizedStartDate);
            }

            if (input.RealizedEndDate != null && input.RealizedEndDate != DateTime.MinValue)
            {
                query = query.Where(x => x.RealizedEndDate <= input.RealizedEndDate.AddDays(1));
            }

            if (!string.IsNullOrEmpty(input.Search))
            {
                query = query.Where(x =>
                    x.Operation.Name.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    x.OperatingRoom.Name.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    x.Operation.OperationType.Branch.Name.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0
                );
            }

            int totalCount = await query.CountAsync();

            if (input.PageSize > 0)
            {
                query = query.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);
            }

            var result = await query.ProjectTo<OperationPlanForReportModel>().ToListAsync();

            return new ResultModel<OvertimeReportOutputModel>
            {
                Result = AutoMapper.Mapper.Map<List<OvertimeReportOutputModel>>(result),
                TotalCount = totalCount,
                Info = new Info { Succeeded = true }
            };

            //return base.GetAsync<TOutputModel>(input, expression);
        }

        public async Task<List<OvertimeReportOutputModel>> GetExportAsync<OvertimeReportOutputModel>(OvertimeReportInputModel input)
        {
            var query = _dataContext.OperationPlans.Where(x => Convert.ToInt32((x.RealizedEndDate - x.RealizedStartDate).TotalMinutes) != x.Operation.OperationTime);

            if (input.BranchId > 0)
            {
                query = query.Where(x => x.Operation.OperationType.BranchId == input.BranchId);
            }

            if (input.DoctorId > 0)
            {
                query = query.Where(x => x.Operation.OperationPersonels.Any(t => t.PersonnelId == input.DoctorId));
            }

            if (input.RealizedStartDate != null && input.RealizedStartDate != DateTime.MinValue)
            {
                query = query.Where(x => x.RealizedStartDate >= input.RealizedStartDate);
            }

            if (input.RealizedEndDate != null && input.RealizedEndDate != DateTime.MinValue)
            {
                query = query.Where(x => x.RealizedEndDate <= input.RealizedEndDate.AddDays(1));
            }

            var list = await query.ProjectTo<OperationPlanForReportModel>().ToListAsync();

            return AutoMapper.Mapper.Map<List<OvertimeReportOutputModel>>(list);
        }
    }
}
