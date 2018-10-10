using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Services.Stores
{
    public class AppointmentCalendarStoreService : StoreService<AppointmentCalendar, AppointmentCalendarModel>, IAppointmentCalendarStoreService
    {
        private DataContext _context;

        public AppointmentCalendarStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AppointmentCalendarModel>> GetAppointmentsByDoctorAndDateAsync(AppointmentDayInputModel model)
        {
            return await GetQueryable().Where(x => x.PersonnelId == model.DoctorId && x.AppointmentDate > model.SelectedDate && x.AppointmentDate < model.SelectedDate.AddDays(1)).ProjectTo<AppointmentCalendarModel>().ToListAsync();
        }

        public async Task<int> GetAppointmentCountByDoctorAndDateTimeAsync(int doctorId, DateTime date)
        {
            return await GetQueryable().CountAsync(x => x.PersonnelId == doctorId && x.AppointmentDate == date);
        }

        public async Task<List<AppointmentCalendarOutputModel>> GetFutureAppointmentListAsync(int doctorId)
        {
            return await GetQueryable().Where(x => x.PersonnelId == doctorId && x.AppointmentDate >= DateTime.Now).ProjectTo<AppointmentCalendarOutputModel>().ToListAsync();
        }
    }
}
