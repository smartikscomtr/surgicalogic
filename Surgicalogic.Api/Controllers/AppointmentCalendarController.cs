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

        public AppointmentCalendarController(IAppointmentCalendarStoreService appointmentStoreService)
        {
            _appointmentCalendarStoreService = appointmentStoreService;
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
        
        [Route("AppointmentCalendar/GetAppointmentCalendarByDate/{selectDate:DateTime}")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanOutputModel>> GetAppointmentCalendarByDate(DateTime selectDate)
        {
            return null;
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