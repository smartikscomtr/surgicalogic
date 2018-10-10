using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IAppointmentCalendarStoreService : IStoreService<AppointmentCalendar, AppointmentCalendarModel>
    {
        Task<List<AppointmentCalendarModel>> GetAppointmentsByDoctorAndDateAsync(AppointmentDayInputModel model);
        Task<int> GetAppointmentCountByDoctorAndDateTimeAsync(int doctorId, DateTime date);
        Task<List<AppointmentCalendarOutputModel>> GetFutureAppointmentListAsync(int doctorId);
    }
}
