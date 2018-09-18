using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.Stores
{
    public class AppointmentCalendarStoreService : StoreService<AppointmentCalendar, AppointmentCalendarModel>, IAppointmentCalendarStoreService
    {
        private DataContext _context;

        public AppointmentCalendarStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
