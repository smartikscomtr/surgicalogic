using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Common.Extensions;
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
        private readonly IPatientStoreService _patientStoreService;

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public AppointmentCalendarController(
            IAppointmentCalendarStoreService appointmentStoreService,
            ISettingStoreService settingStoreService,
            IPatientStoreService patientStoreService)
        {
            _appointmentCalendarStoreService = appointmentStoreService;
            _settingStoreService = settingStoreService;
            _patientStoreService = patientStoreService;
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

            var start = workingHourStart.TimeValue.HourToDateTime();
            var end = workingHourEnd.TimeValue.HourToDateTime();
            var personPerPeriod = Convert.ToInt32(personPerPeriodSetting.IntValue);

            var appointments = await _appointmentCalendarStoreService.GetAppointmentsByDoctorAndDateAsync(model);

            var disabledTimes = appointments.GroupBy(x => x.AppointmentDate).Where(x => x.Count() >= personPerPeriod).Select(x => x.Key.ToString("HH:mm")).ToList();

            //ön yüzde kullandığımız component başlangıç dakikası almadığı için saat başından başlangıç dakikasına kadar olan slotları kapatıyorum. 
            if (start.Minute > 0)
            {
                string hour = start.ToString("HH");
                int minute = 0;

                while(minute < start.Minute)
                {
                    disabledTimes.Add(hour + ":" + (minute <= 9 ? "0" + minute : minute.ToString()));
                    minute += interval;
                }
            }

            //ön yüzde kullandığımız component bitiş dakikası almadığı için bitiş dakikasından sonraki slotları kapatıyorum. 
            if (end.Minute > 0)
            {
                string hour = end.ToString("HH");
                int minute = 60 - interval;

                while (minute + interval > end.Minute)
                {
                    disabledTimes.Add(hour + ":" + (minute <= 9 ? "0" + minute : minute.ToString()));
                    minute -= interval;
                }

                end = end.AddHours(1);
            }

            return new AppointmentDayOutputModel
            {
                Interval = interval,
                StartTime = start.Hour,
                EndTime = end.Hour,
                Disabled = disabledTimes.ToArray(),
                PersonPerPeriod = personPerPeriod,
                SelectedTimes = appointments.Select(x => x.AppointmentDate.ToString("HH:mm")).ToArray()
            };
        }

        [Route("AppointmentCalendar/GetFutureAppointmentListAsync/{doctorId:int}")]
        [HttpGet]
        public async Task<List<AppointmentCalendarOutputModel>> GetFutureAppointmentListAsync(int doctorId)
        {
            return await _appointmentCalendarStoreService.GetFutureAppointmentListAsync(doctorId);
        }

        [Route("AppointmentCalendar/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("AppointmentCalendar_{0}.xlsx", Guid.NewGuid().ToString());

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
            var result = new ResultModel<AppointmentCalendarOutputModel> {Info = new Info { Succeeded = false } };

            await semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(5));
            try
            {
                var appointmentDateTime = new DateTime(item.AppointmentDate.Year, item.AppointmentDate.Month, item.AppointmentDate.Day, Convert.ToInt32(item.AppointmentTime.Split(':')[0]), Convert.ToInt32(item.AppointmentTime.Split(':')[1]), 0);

                var appointmentAvailable = await CheckAppointmentAsync(item.PersonnelId, appointmentDateTime);

                if (!appointmentAvailable)
                {
                    result.Info.Message = MessageType.AppointmentIsNotAvailable;
                    return result;
                }

                var patientItem = new PatientModel()
                {
                    IdentityNumber = item.IdentityNumber,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Phone = item.Phone,
                    Address = item.Address
                };

                var patient = await _patientStoreService.InsertAndSaveAsync<PatientOutputModel>(patientItem);

                if (patient.Info.Succeeded)
                {
                    var appointmentCalendarItem = new AppointmentCalendarModel()
                    {
                        AppointmentDate = appointmentDateTime,
                        PatientId = patient.Result.Id,
                        PersonnelId = item.PersonnelId
                    };

                    result = await _appointmentCalendarStoreService.InsertAndSaveAsync<AppointmentCalendarOutputModel>(appointmentCalendarItem);
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }

            return result;
        }

        private async Task<bool> CheckAppointmentAsync(int doctorId, DateTime date)
        {
            var systemSetting = await _settingStoreService.GetByKeyAsync(SettingKey.ClinicPersonPerPeriod.ToString());
            var personPerPeriod = systemSetting.IntValue;

            var appointmentCount = await _appointmentCalendarStoreService.GetAppointmentCountByDoctorAndDateTimeAsync(doctorId, date);

           return personPerPeriod > appointmentCount ? true : false;
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