using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Contracts.Stores.IReportStoreService;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.ExportModel.Report;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class ReportController : Controller
    {
        private readonly IOvertimeReportStoreService _overtimeReportStoreService;
        private readonly IOperationPlanHistoryStoreService _operationPlanHistoryStoreService;

        public ReportController(
            IOvertimeReportStoreService overtimeReportStoreService,
            IOperationPlanHistoryStoreService operationPlanHistoryStoreService
            )
        {
            _overtimeReportStoreService = overtimeReportStoreService;
            _operationPlanHistoryStoreService = operationPlanHistoryStoreService;
        }

        [HttpGet]
        [Route("Report/OvertimeReportPage")]
        public async Task<ResultModel<OvertimeReportOutputModel>> OvertimeReportPage(OvertimeReportInputModel input)
        {
            return await _overtimeReportStoreService.GetAsync<OvertimeReportOutputModel>(input);
        }

        [Route("Report/OvertimeReportExcelExport")]
        public async Task<string> OvertimeReportExcelExport(OvertimeReportInputModel input)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Overtime_Operations_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _overtimeReportStoreService.GetExportAsync<OvertimeReportOutputModel>(input);

            excelService.Write(fs, "Worksheet", typeof(OvertimeReportOutputModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [Route("OperationPlan/GetOperationPlanHistory")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetOperationPlanHistory(HistoryPlanningInputModel input)
        {
            return await _operationPlanHistoryStoreService.GetAsync<OperationPlanHistoryOutputModel>(input);
        }

        [Route("OperationPlan/GetTomorrowOperationList")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationList(GridInputModel input)
        {
            return await _operationPlanHistoryStoreService.GetTomorrowOperationListAsync(input);
        }

        [Route("Report/HistoryPlanningReportExcelExport")]
        public async Task<string> HistoryPlanningReportExcelExport(HistoryPlanningInputModel input)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("History_Operations_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operationPlanHistoryStoreService.GetExportAsync<HistoryPlanningReportExportModel>(input);

            excelService.Write(fs, "Worksheet", typeof(HistoryPlanningReportExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }
    }
}