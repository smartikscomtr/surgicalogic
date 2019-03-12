using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
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
    [Authorize]
    public class AppointmentCalendarController : Controller
    {
        private readonly IAppointmentCalendarStoreService _appointmentCalendarStoreService;
        private readonly ISettingStoreService _settingStoreService;
        private readonly IPatientStoreService _patientStoreService;
        private readonly IOperationPlanStoreService _operationPlanStoreService;

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private static List<int> availableAppointmentTimes = new List<int>();
        private static DateTime start = new DateTime();
        private static DateTime end = new DateTime();

        public AppointmentCalendarController(
            IAppointmentCalendarStoreService appointmentStoreService,
            ISettingStoreService settingStoreService,
            IPatientStoreService patientStoreService,
            IOperationPlanStoreService operationPlanStoreService)
        {
            _appointmentCalendarStoreService = appointmentStoreService;
            _settingStoreService = settingStoreService;
            _patientStoreService = patientStoreService;
            _operationPlanStoreService = operationPlanStoreService;
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


            //Ortalama süre düzeltme(dk)
            var patientsFailureRate = (double)systemSettings.SingleOrDefault(x => x.Key == SettingKey.PatientsFailureRate.ToString()).IntValue.Value / 100;
            var withoutAppointmentPatientRate = (double)systemSettings.SingleOrDefault(x => x.Key == SettingKey.WithoutAppointmentPatientRate.ToString()).IntValue.Value / 100;
            var forAverageTimeEveryPatient = systemSettings.SingleOrDefault(x => x.Key == SettingKey.ForAverageTimeEveryPatient.ToString()).IntValue.Value;
            var operationPlanPeriod = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationPeriodInMinutes.ToString()).IntValue.Value;

            var subTotal = Math.Round((1 - patientsFailureRate + withoutAppointmentPatientRate) * forAverageTimeEveryPatient, 2);


            //Sigma Değerleri
            var forStandardDeviationAverageTime = systemSettings.SingleOrDefault(x => x.Key == SettingKey.ForStandardDeviationAverageTime.ToString()).DoubleValue.Value;

            var sigma1 = Math.Round((1 - patientsFailureRate - withoutAppointmentPatientRate) * (Math.Pow(forStandardDeviationAverageTime, 2) + Math.Pow((patientsFailureRate - withoutAppointmentPatientRate), 2) * Math.Pow(forAverageTimeEveryPatient, 2)), 5);
            var sigma2 = Math.Round(patientsFailureRate * Math.Pow((1 - patientsFailureRate + withoutAppointmentPatientRate), 2) * Math.Pow(forAverageTimeEveryPatient, 2), 5);
            var sigma3 = Math.Round(withoutAppointmentPatientRate * (2 * Math.Pow(forStandardDeviationAverageTime, 2) + Math.Pow((1 + patientsFailureRate - withoutAppointmentPatientRate), 2) * Math.Pow(forAverageTimeEveryPatient, 2)), 5);


            //Standart Sapma Düzeltme
            var editStandardDeviation = Math.Round(sigma1 + sigma2 + sigma3, 5);


            //N
            var sumPatientNumber = systemSettings.SingleOrDefault(x => x.Key == SettingKey.SumPatientNumber.ToString()).IntValue.Value;
            var N = Math.Ceiling(sumPatientNumber / (1 - patientsFailureRate + withoutAppointmentPatientRate));


            //k
            var doctorTimePatientTimeRate = systemSettings.SingleOrDefault(x => x.Key == SettingKey.DoctorTimePatientTimeRate.ToString()).IntValue.Value;

            var k1 = 0.9973;
            var k2 = -0.103 * (0.005765 * doctorTimePatientTimeRate * (1 - patientsFailureRate) + Math.Pow((doctorTimePatientTimeRate * (1 - patientsFailureRate)), -0.3481));
            var k3 = -0.10699 * Math.Pow((forStandardDeviationAverageTime / subTotal), 1.257);
            var k4 = -0.627 * Math.Pow((N * (1 - patientsFailureRate)), -0.8579);
            var k5 = -0.007574 * (Math.Pow((Math.Abs(doctorTimePatientTimeRate * (1 - withoutAppointmentPatientRate) - 2.143)), 0.9682) - 0.622 * doctorTimePatientTimeRate * (1 - withoutAppointmentPatientRate));
            var k6 = 0.004855 * Math.Pow(doctorTimePatientTimeRate, 0.8913);

            var k = Math.Round(Math.Pow((k1 + k2 + k3 + k4 + k5 + k6), -1.898), 6);

            //calculatedSchedule ve assignedSchedule
            var roundingIntervalValue = systemSettings.SingleOrDefault(x => x.Key == SettingKey.RoundingIntervalValue.ToString()).IntValue.Value;

            var calculatedSchedule = 0.0;
            var assignedSchedule = new int[sumPatientNumber];

            for (int i = 1; i <= sumPatientNumber; i++)
            {
                if ((k * (i - 1) * subTotal - Math.Sqrt(editStandardDeviation) * ((N + i) / (N - 1)) * Math.Sqrt(i)) < 0)
                {
                    calculatedSchedule = 0.0;
                }
                else
                {
                    calculatedSchedule = Math.Round(k * (i - 1) * subTotal - Math.Sqrt(editStandardDeviation) * ((N + i) / (N - 1)) * Math.Sqrt(i), 2);
                };

                assignedSchedule[i - 1] = (int)Math.Round((calculatedSchedule / (double)roundingIntervalValue), MidpointRounding.AwayFromZero) * roundingIntervalValue;
            }

            availableAppointmentTimes = assignedSchedule.ToList();

            start = workingHourStart.TimeValue.HourToDateTime();
            end = workingHourEnd.TimeValue.HourToDateTime();
            var intervalStartMinuteDifference = 0;
            var personPerPeriod = assignedSchedule.GroupBy(x => x).OrderByDescending(x => x.Count()).FirstOrDefault().Count();
            var intervals = new int[Convert.ToInt32((end - start).TotalMinutes) / roundingIntervalValue];

            for (int i = 0; i < intervals.Length; i++)
            {
                intervals[i] = i * roundingIntervalValue;
            }

            //Eğer başlangıç dakikası, her randevuya ayrılacak sürenin bir katı değilse
            if (start.Minute > 0 && start.Minute % roundingIntervalValue > 0)
            {
                intervalStartMinuteDifference += roundingIntervalValue - start.Minute % roundingIntervalValue;
            }

            var disabledTimes = new List<string>();

            var appointments = await _appointmentCalendarStoreService.GetAppointmentsByDoctorAndDateAsync(model);
            var selectedTimesAsDate = appointments.Select(x => x.AppointmentDate).ToArray();

            var assignedSchedulesAsDate = new string[assignedSchedule.Length];

            for (int i = 0; i < assignedSchedule.Length; i++)
            {
                assignedSchedulesAsDate[i] = start.AddMinutes(intervalStartMinuteDifference + assignedSchedule[i]).ToString("HH:mm");
            }


            for (int i = 0; i < selectedTimesAsDate.Length; i++)
            {
                disabledTimes.Add(selectedTimesAsDate[i].ToString("HH:mm"));
            }

            foreach (var item in assignedSchedulesAsDate)
            {
                if (assignedSchedulesAsDate.Count(x => x == item) > disabledTimes.Count(x => x == item))
                {
                    disabledTimes.Remove(item);
                }
            }

            var operationPlans = await _operationPlanStoreService.GetOperationPlansByDoctorAndDateAsync(model);

            foreach (var item in operationPlans)
            {
                for (DateTime i = item.RealizedStartDate; i <= item.RealizedEndDate; i = i.AddMinutes(1))
                {
                    disabledTimes.Add(i.ToString("HH:mm"));
                }
            }

            for (int i = 0; i < selectedTimesAsDate.Length; i++)
            {
                var minutesFromStart = (selectedTimesAsDate[i] - new DateTime(selectedTimesAsDate[i].Year, selectedTimesAsDate[i].Month, selectedTimesAsDate[i].Day, start.Hour, start.Minute, 0)).TotalMinutes;
                int numIndex = Array.IndexOf(assignedSchedule, Convert.ToInt32(minutesFromStart));
                assignedSchedule = assignedSchedule.Where((val, idx) => idx != numIndex).ToArray();
                numIndex = Array.IndexOf(assignedSchedulesAsDate, selectedTimesAsDate[i].ToString("HH:mm"));
                assignedSchedulesAsDate = assignedSchedulesAsDate.Where((val, idx) => idx != numIndex).ToArray();
            }

            var notAvailableItems = intervals.Except(assignedSchedule).ToArray();
            for (int i = 0; i < notAvailableItems.Count(); i++)
            {
                var disabledTime = start.AddMinutes(notAvailableItems[i]);
                disabledTimes.Add(disabledTime.ToString("HH:mm"));
            }

            //ön yüzde kullandığımız component başlangıç dakikası almadığı için saat başından başlangıç dakikasına kadar olan slotları kapatıyorum. 
            if (start.Minute > 0)
            {
                string hour = start.ToString("HH");
                int minute = 0;

                while (minute < start.Minute)
                {
                    disabledTimes.Add(hour + ":" + (minute <= 9 ? "0" + minute : minute.ToString()));
                    minute += roundingIntervalValue;
                }
            }

            //ön yüzde kullandığımız component bitiş dakikası almadığı için bitiş dakikasından sonraki slotları kapatıyorum. 
            if (end.Minute > 0)
            {
                string hour = end.ToString("HH");
                int minute = 60 - roundingIntervalValue;

                while (minute + roundingIntervalValue > end.Minute)
                {
                    disabledTimes.Add(hour + ":" + (minute <= 9 ? "0" + minute : minute.ToString()));
                    minute -= roundingIntervalValue;
                }

                end = end.AddHours(1);
            }

            return new AppointmentDayOutputModel
            {
                Interval = roundingIntervalValue,
                StartTime = start.Hour,
                EndTime = end.Hour,
                Disabled = disabledTimes.ToArray(),
                PersonPerPeriod = personPerPeriod,
                SelectedTimes = appointments.Select(x => x.AppointmentDate.ToString("HH:mm")).ToArray(),
                AssignedSchedulesAsDate = assignedSchedulesAsDate
            };
        }

        [Route("AppointmentCalendar/GetFutureAppointmentListAsync")]
        [HttpGet]
        public async Task<ResultModel<AppointmentCalendarOutputModel>> GetFutureAppointmentListAsync(AppointmentDayInputModel model)
        {
            var result = await _appointmentCalendarStoreService.GetFutureAppointmentListAsync(model);
            return result;
        }

        [Route("AppointmentCalendar/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("AppointmentCalendar_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _appointmentCalendarStoreService.GetExportAsync<AppointmentCalendarExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(AppointmentCalendarExportModel), System.Globalization.CultureInfo.CurrentCulture);

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
            var result = new ResultModel<AppointmentCalendarOutputModel> { Info = new Info { Succeeded = false } };

            await semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(5));
            try
            {
                var systemSettings = await _settingStoreService.GetAllAsync();
                var appointmentDateTime = new DateTime(item.AppointmentDate.Year, item.AppointmentDate.Month, item.AppointmentDate.Day, Convert.ToInt32(item.AppointmentTime.Split(':')[0]), Convert.ToInt32(item.AppointmentTime.Split(':')[1]), 0);

                var intervalStartMinuteDifference = 0;
                var minutesFromStart = (appointmentDateTime - new DateTime(appointmentDateTime.Year, appointmentDateTime.Month, appointmentDateTime.Day, start.Hour, start.Minute, 0)).TotalMinutes;

                var roundingIntervalValue = systemSettings.SingleOrDefault(x => x.Key == SettingKey.RoundingIntervalValue.ToString()).IntValue.Value;
                //Eğer başlangıç dakikası, her randevuya ayrılacak sürenin bir katı değilse
                if (start.Minute > 0 && start.Minute % roundingIntervalValue > 0)
                {
                    intervalStartMinuteDifference += roundingIntervalValue - start.Minute % roundingIntervalValue;
                }

                var appointmentAvailable = await CheckAppointmentAsync(item.PersonnelId, appointmentDateTime, availableAppointmentTimes.Count(x => x == minutesFromStart - intervalStartMinuteDifference));

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

        private async Task<bool> CheckAppointmentAsync(int doctorId, DateTime date, int personPerPeriod)
        {
            var appointmentCount = await _appointmentCalendarStoreService.GetAppointmentCountByDoctorAndDateTimeAsync(doctorId, date);

            return personPerPeriod > appointmentCount ? true : false;
        }

        /// <summary>
        /// Remove appointmentCalendar item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("AppointmentCalendar/DeleteAppointment/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteAppointment(int id)
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