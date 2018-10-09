using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class AppointmentCalendarController : Controller
    {
        private readonly IAppointmentCalendarStoreService _appointmentCalendarStoreService;
        private readonly ISettingStoreService _settingStoreService;

        public AppointmentCalendarController(
            IAppointmentCalendarStoreService appointmentStoreService,
            ISettingStoreService settingStoreService)
        {
            _appointmentCalendarStoreService = appointmentStoreService;
            _settingStoreService = settingStoreService;
        }

        /// <summary>
        /// Get appointmentCalendar methode
        /// </summary>
        /// <returns>AppointmentCalendarOutputModel list</returns>
        [Route("AppointmentCalendar/GetAppointmentCalendars")]
        public async Task<ResultModel<AppointmentCalendarOutputModel>> GetAppointmentCalendar(GridInputModel input)
        {
            return await _appointmentCalendarStoreService.GetAsync<AppointmentCalendarOutputModel>(input);
        }

        [Route("AppointmentCalendar/GetAllAppointmentCalendars")]
        public async Task<ResultModel<AppointmentCalendarOutputModel>> GetAllAppointmentCalendars()
        {
            return await _appointmentCalendarStoreService.GetAsync<AppointmentCalendarOutputModel>();
        }

        [Route("AppointmentCalendar/GetAppointmentCalendarByDate")]
        [HttpGet]
        public async Task<AppointmentDayOutputModel> GetAppointmentCalendarByDate(AppointmentDayInputModel model)
        {
            var systemSettings = await _settingStoreService.GetAllAsync();

            var workingHourStart = systemSettings.SingleOrDefault(x => x.Key == SettingKey.ClinicWorkingHourStart.ToString());
            var workingHourEnd = systemSettings.SingleOrDefault(x => x.Key == SettingKey.ClinicWorkingHourEnd.ToString());
            var personPerPeriodSetting = systemSettings.SingleOrDefault(x => x.Key == SettingKey.ClinicPersonPerPeriod.ToString());
            var interval = systemSettings.SingleOrDefault(x => x.Key == SettingKey.ClinicPeriodInMinutes.ToString()).IntValue.Value;

            var start =  Convert.ToInt32(workingHourStart.TimeValue.Split(':')[0]);
            var end = Convert.ToInt32(workingHourEnd.TimeValue.Split(':')[0]);
            var personPerPeriod = Convert.ToInt32(personPerPeriodSetting.IntValue);

            var appointments = await _appointmentCalendarStoreService.GetAppointmentsByDoctorAndDateAsync(model);

            return new AppointmentDayOutputModel
            {
                Interval = interval,
                StartTime = start,
                EndTime = end,
                Disabled = appointments.GroupBy(x => x.AppointmentDate).Where(x => x.Count() >= personPerPeriod).Select(x => x.Key.ToString("HH:mm")).ToArray(),
                PersonPerPeriod = personPerPeriod,
                SelectedTimes = appointments.Select(x => x.AppointmentDate.ToString("HH:mm")).ToArray()
            };
        }

        [Route("AppointmentCalendar/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("AppointmentCalendard_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _appointmentCalendarStoreService.GetExportAsync<AppointmentCalendarExportModel>();

            excelService.Write(fs, "Worksheet", typeof(AppointmentCalendarExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add appointmentCalendar methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>AppointmentCalendarOutputModel</returns>
        [Route("AppointmentCalendar/InsertAppointmentCalendar")]
        [HttpPost]
        public async Task<ResultModel<AppointmentCalendarOutputModel>> InsertAppointmentCalendar([FromBody] AppointmentCalendarInputModel item)
        {
            var appointmentCalendarItem = new AppointmentCalendarModel()
            {
                AppointmentDate = item.AppointmentDate,
                PatientId = item.PatientId,
                PersonnelId = item.PersonnelId
            };

            return await _appointmentCalendarStoreService.InsertAndSaveAsync<AppointmentCalendarOutputModel>(appointmentCalendarItem);
        }

        /// <summary>
        /// Remove appointmentCalendar item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("AppointmentCalendar/DeleteAppointmentCalendar/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteAppointmentCalendar(int id)
        {
            return await _appointmentCalendarStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update appointmentCalendar methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>AppointmentCalendarModel</returns>
        [Route("AppointmentCalendar/UpdateAppointmentCalendar")]
        [HttpPost]
        public async Task<ResultModel<AppointmentCalendarOutputModel>> UpdateAppointmentCalendar(AppointmentCalendarInputModel item)
        {
            var appointmentCalendarModel = new AppointmentCalendarModel()
            {
                AppointmentDate = item.AppointmentDate,
                PatientId = item.PatientId,
                PersonnelId = item.PersonnelId
            };

            return await _appointmentCalendarStoreService.UpdateAndSaveAsync<AppointmentCalendarOutputModel>(appointmentCalendarModel);
        }
    }
}