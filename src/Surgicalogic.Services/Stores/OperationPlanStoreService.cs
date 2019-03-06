using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
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

        public async Task DeletePlanByDateAsync(DateTime date)
        {
            var twoDaysLater = date.AddDays(1);

            var items = await GetQueryable().Where(x => x.OperationDate > date && x.OperationDate < twoDaysLater).ToListAsync();

            foreach (var item in items)
            {
                await DeleteByIdAsync(item.Id);
            }

            await SaveChangesAsync();
        }

        public async Task<List<OperationPlanOutputModel>> GetDashboardTimelineOperationsAsync(DateTime selectDate)
        {
            var date = new DateTime(selectDate.Year, selectDate.Month, selectDate.Day, 0, 0, 0);

            return await GetQueryable().Where(x => x.OperationDate > date && x.OperationDate < date.AddDays(1)).ProjectTo<OperationPlanOutputModel>().ToListAsync();
        }

        public async Task<List<OperationPlanModel>> GetByIdListAsync(int[] updatedItemIds)
        {
            return await GetQueryable().Where(x => updatedItemIds.Contains(x.Id)).ProjectTo<OperationPlanModel>().ToListAsync();
        }

        public async Task<List<SimulationOperationPlanModel>> GetOperationByIdListAsync(DateTime selectDate)
        {
            var date = new DateTime(selectDate.Year, selectDate.Month, selectDate.Day, 0, 0, 0);

            return await GetQueryable().Where(x => x.IsActive && x.OperationDate > date && x.OperationDate < date.AddDays(1)).OrderBy(x => x.OperationDate).ThenBy(x => x.OperatingRoomId)
                .ProjectTo<SimulationOperationPlanModel>().ToListAsync();
        }

        public async Task<List<OperationPlanModel>> GetOperationPlansByDoctorAndDateAsync(AppointmentDayInputModel model)
        {
            return await GetQueryable().Where(x => x.Operation.OperationPersonels.Any(y => y.PersonnelId == model.DoctorId) && x.RealizedStartDate >= model.SelectedDate && x.RealizedStartDate < model.SelectedDate.AddDays(1)).ProjectTo<OperationPlanModel>().ToListAsync();
        }
    }
}