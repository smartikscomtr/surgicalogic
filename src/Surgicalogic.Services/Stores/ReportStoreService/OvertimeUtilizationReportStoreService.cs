using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Contracts.Stores.IReportStoreService;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Model.ExportModel.Report;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores.ReportStoreService
{
    public class OvertimeUtilizationReportStoreService : IOvertimeUtilizationReportStoreService
    {
        private DataContext _context;
        private readonly ISettingStoreService _settingStoreService;

        public OvertimeUtilizationReportStoreService(DataContext context, ISettingStoreService settingStoreService)
        {
            _context = context;
            _settingStoreService = settingStoreService;
        }

        public async Task<List<OvertimeUtilizationForOvertimeReportOutputModel>> GetOvertimeAndUtilizationAsync(DateTime operationStartDate, DateTime operationEndDate)
        {
            var operationPlans = _context.OperationPlans.Where(x => x.IsActive);

            if (operationStartDate != null && operationStartDate > DateTime.MinValue)
            {
                operationPlans = operationPlans.Where(x => x.OperationDate >= operationStartDate);
            }

            if (operationEndDate != null && operationEndDate > DateTime.MinValue)
            {
                operationPlans = operationPlans.Where(x => x.OperationDate <= operationEndDate.AddDays(1));
            }
                
            var allOperations = await operationPlans.ProjectTo<OperationPlanModel>().ToListAsync();

            var overtimeOperations = allOperations.Where(x => Convert.ToInt32((x.RealizedEndDate - x.RealizedStartDate).TotalMinutes) != x.Operation.OperationTime).ToList();

            var operatingRooms = allOperations.Select(x => x.OperatingRoom).ToList(); 

            var result = new List<OvertimeUtilizationForOvertimeReportOutputModel>();

            var workingHours = await GetWorkingHours(operationStartDate, operationEndDate);

            foreach (var item in operatingRooms.Select(x => x.Id).Distinct())
            {
                var operations = overtimeOperations.Where(x => x.OperatingRoomId == item)
                    .Select
                    (
                        x => new OvertimeUtilizationForDateDifferenceReportOutputModel
                        {
                            OperationId = x.OperationId,
                            DateDifference = Convert.ToInt32((x.RealizedEndDate - x.RealizedStartDate).TotalMinutes) - x.Operation.OperationTime
                        }
                    ).ToList();

                result.Add(new OvertimeUtilizationForOvertimeReportOutputModel
                    {
                        OperatingRoomId = item,
                        OperatingRoom = operatingRooms.Where(x => x.Id == item).First().Name,
                        Overtime = operations.Count == 0 ? 0 : operations.Sum(x => x.DateDifference) / operations.Count,
                        Utilization = Math.Round((allOperations.Where(x => x.OperatingRoomId == item).Sum(x => (x.RealizedEndDate - x.RealizedStartDate).TotalHours) / workingHours) * 100, 2)
                    }
                );
            }

            return result;
        }
        public async Task<ResultModel<OvertimeUtilizationReportOutputModel>> GetAsync<TOutputModel>(OvertimeUtilizationReportInputModel input)
        {
            var result = await GetOvertimeAndUtilizationAsync(input.OperationStartDate, input.OperationEndDate);

            if (!string.IsNullOrEmpty(input.SortBy))
            {
                Func<OvertimeUtilizationForOvertimeReportOutputModel, object> orderBy = null;

                switch (input.SortBy)
                {
                    case "operatingRoom":
                        orderBy = x => x.OperatingRoom;
                        break;
                    case "overtime":
                        orderBy = x => x.Overtime;
                        break;
                    case "utilization":
                        orderBy = x => x.Utilization;
                        break;
                    default:
                        orderBy = x => x.OperatingRoomId;
                        break;
                }

                if (input.Descending == true)
                {
                    result = result.OrderByDescending(orderBy).ToList();
                }
                else
                {
                    result = result.OrderBy(orderBy).ToList();
                }
            }
            
            int totalCount = result.Count();

            if (input.PageSize > 0)
            {
                result = result.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize).ToList();
            }

            return new ResultModel<OvertimeUtilizationReportOutputModel>
            {
                Result = AutoMapper.Mapper.Map<List<OvertimeUtilizationReportOutputModel>>(result),
                TotalCount = 0,
                Info = new Info { Succeeded = true }
            };
        }

        private async Task<int> GetWorkingHours(DateTime operationStartDate, DateTime operationEndDate)
        {
            var systemSettings = await _settingStoreService.GetAllAsync();

            var workingHourStart = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationWorkingHourStart.ToString()).TimeValue.HourToDateTime();
            var workingHourEnd = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationWorkingHourEnd.ToString()).TimeValue.HourToDateTime();

            var dailyWorkingHours = (workingHourEnd - workingHourStart).Hours;

            var dayCount = GetWeekDaysCount(operationStartDate, operationEndDate);

            return dailyWorkingHours * dayCount;
        }

        private int GetWeekDaysCount(DateTime operationStartDate, DateTime operationEndDate)
        {
            var result = 0;
            for (DateTime i = operationStartDate; i <= operationEndDate; i = i.AddDays(1))
            {
                if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                {
                    result++;
                }
            }
            return result;
        }

        public async Task<List<OvertimeUtilizationReportExportModel>> GetExportAsync(OvertimeUtilizationReportInputModel input)
        {
            var query = await GetOvertimeAndUtilizationAsync(input.OperationStartDate, input.OperationEndDate);

            return AutoMapper.Mapper.Map<List<OvertimeUtilizationReportExportModel>>(query);
        }
    }
}
