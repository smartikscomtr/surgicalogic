using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class OperatingRoomCalendarController : Controller
    {
        private readonly IOperatingRoomCalendarStoreService _operatingRoomCalendarStoreService;
        public OperatingRoomCalendarController(IOperatingRoomCalendarStoreService operatingRoomCalendarStoreService)
        {
            _operatingRoomCalendarStoreService = operatingRoomCalendarStoreService;
        }

        [Route("OperatingRoomCalendar/GetOperatingRoomCalendar")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> GetOperatingRoomCalendar(OperatingRoomCalendarInputModel item)
        {
            return await _operatingRoomCalendarStoreService.GetByOperatingRoomIdAsync(item.OperatingRoomId);
        }


        [Route("OperatingRoomCalendar/ExcelExport/{id:int}")]
        public async Task<string> ExcelExport(int id)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("OperatingRoomCalendars_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operatingRoomCalendarStoreService.GetExportAsync<OperatingRoomCalendarExportModel>(id);

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(OperatingRoomCalendarExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [Route("OperatingRoomCalendar/InsertOperatingRoomCalendar")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> InsertOperatingRoomCalendar([FromBody] OperatingRoomCalendarInputModel item)
        {
            var model = new OperatingRoomCalendarModel
            {
                OperatingRoomId = item.OperatingRoomId,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };

            return await _operatingRoomCalendarStoreService.InsertAndSaveAsync<OperatingRoomCalendarOutputModel>(model);
        }

        [Route("OperatingRoomCalendar/UpdateOperatingRoomCalendar")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> UpdateOperatingRoomCalendar([FromBody] OperatingRoomCalendarInputModel item)
        {
            var model = new OperatingRoomCalendarModel
            {
                Id=item.Id,
                OperatingRoomId = item.OperatingRoomId,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };

            return await _operatingRoomCalendarStoreService.UpdateAndSaveAsync<OperatingRoomCalendarOutputModel>(model);
        }

        [Route("OperatingRoomCalendar/DeleteOperatingRoomCalendar/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperatingRoomCalendar(int id)
        {
            return await _operatingRoomCalendarStoreService.DeleteAndSaveByIdAsync(id);
        }
    }
}