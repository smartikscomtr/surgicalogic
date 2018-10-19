using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomCalendarStoreService : StoreService<OperatingRoomCalendar, OperatingRoomCalendarModel>, IOperatingRoomCalendarStoreService
    {
        private DataContext _context;
        public OperatingRoomCalendarStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> GetByOperatingRoomIdAsync(int operatingRoomId)
        {
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var result = await GetQueryable().Where(x => x.OperatingRoomId == operatingRoomId && x.EndDate > today).ProjectTo<OperatingRoomCalendarModel>().ToListAsync();

            return new ResultModel<OperatingRoomCalendarOutputModel>
            {
                Result = Mapper.Map<IEnumerable<OperatingRoomCalendarOutputModel>>(result),
                TotalCount = result.Count,
                Info = new Info { Succeeded = true }
            };
        }

        public async Task<List<OperationRoomCalendarExportModel>> GetExportAsync<OperationRoomCalendarExportModel>(int id)
        {
            var query = GetQueryable();
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            query = query.Where(x => x.OperatingRoomId == id && x.EndDate > today);

            var projectQuery = query.ProjectTo<OperationRoomCalendarExportModel>();
            return await projectQuery.ToListAsync();
        }
    }
}