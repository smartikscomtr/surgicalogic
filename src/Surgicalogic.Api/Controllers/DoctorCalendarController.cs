using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;
using Smartiks.Framework.IO;
using System.IO;
using System;
using System.Collections.Generic;
using Surgicalogic.Model.ExportModel;
using Microsoft.AspNetCore.Authorization;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Common.Settings;

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class DoctorCalendarController : Controller
    {
        private readonly IDoctorCalendarStoreService _doctorCalendarStoreService;

        public DoctorCalendarController(IDoctorCalendarStoreService doctorCalendarStoreService)
        {
            _doctorCalendarStoreService = doctorCalendarStoreService;
        }

        /// <summary>
        /// Get doctorCalendar methode
        /// </summary>
        /// <returns>DoctorCalendarOutputModel list</returns>
        [Route("DoctorCalendar/GetDoctorCalendars")]
        [HttpGet]
        public async Task<ResultModel<DoctorCalendarOutputModel>> GetDoctorCalendar(GridInputModel input)
        {
            return await _doctorCalendarStoreService.GetAsync<DoctorCalendarOutputModel>(input);
        }

        [Route("DoctorCalendar/GetAllDoctorCalendars")]
        public async Task<ResultModel<DoctorCalendarOutputModel>> GetAllDoctorCalendars()
        {
            return await _doctorCalendarStoreService.GetAsync<DoctorCalendarOutputModel>();
        }

        [Route("DoctorCalendar/ExcelExport")]
        public async Task<string> ExcelExport(string langId)
        {
            AppSettings.SetSiteLanguage(langId);

            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("DoctorCalendars_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _doctorCalendarStoreService.GetExportAsync<DoctorCalendarExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(DoctorCalendarExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add doctorCalendar methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>DoctorCalendarOutputModel</returns>
        [Route("DoctorCalendar/InsertDoctorCalendar")]
        [HttpPost]
        public async Task<ResultModel<DoctorCalendarOutputModel>> InsertDoctorCalendar([FromBody] DoctorCalendarInputModel item)
        {
            var doctorCalendarItem = new DoctorCalendarModel()
            {
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                PersonnelId = item.PersonnelId
            };

            return await _doctorCalendarStoreService.InsertAndSaveAsync<DoctorCalendarOutputModel>(doctorCalendarItem);
        }

        /// <summary>
        /// Remove doctorCalendar item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("DoctorCalendar/DeleteDoctorCalendar/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteDoctorCalendar(int id)
        {
            return await _doctorCalendarStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update doctorCalendar methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>DoctorCalendarModel</returns>
        [Route("DoctorCalendar/UpdateDoctorCalendar")]
        [HttpPost]
        public async Task<ResultModel<DoctorCalendarOutputModel>> UpdateDoctorCalendar([FromBody] DoctorCalendarInputModel item)
        {
            var doctorCalendarItem = new DoctorCalendarModel()
            {
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                PersonnelId = item.PersonnelId
            };

            return await _doctorCalendarStoreService.UpdateAndSaveAsync<DoctorCalendarOutputModel>(doctorCalendarItem);
        }
    }
}