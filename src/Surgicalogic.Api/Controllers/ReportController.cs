using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Contracts.Stores.IReportStoreService;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class ReportController : Controller
    {
        private readonly IOvertimeReportStoreService _overtimeReportStoreService;

        public ReportController(IOvertimeReportStoreService overtimeReportStoreService)
        {
            _overtimeReportStoreService = overtimeReportStoreService;
        }

        [HttpGet]
        [Route("Report/OvertimeReportPage")]
        public async Task<ResultModel<OvertimeReportOutputModel>> OvertimeReportPage(OvertimeReportInputModel input)
        {
            var result = await _overtimeReportStoreService.GetAsync<OvertimeReportOutputModel>(input);
            return result;
        }

        [Route("Report/OvertimeReportExcelExport")]
        public async Task<string> ExcelExport(OvertimeReportInputModel input)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Overtime_Operations_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _overtimeReportStoreService.GetExportAsync<OvertimeReportOutputModel>(input);

            excelService.Write(fs, "Worksheet", typeof(OvertimeReportOutputModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }
    }
}