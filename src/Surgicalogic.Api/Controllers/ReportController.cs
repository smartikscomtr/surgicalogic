using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Contracts.Stores.IReportStoreService;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.ExportModel.Report;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IOvertimeReportStoreService _overtimeReportStoreService;
        private readonly IOperationPlanHistoryStoreService _operationPlanHistoryStoreService;
        private readonly IHistoryClinicReportStoreService _historyClinicReportStoreService;
        private readonly IOvertimeUtilizationReportStoreService _overtimeUtilizationStoreService;

        public ReportController(
            IOvertimeReportStoreService overtimeReportStoreService,
            IOperationPlanHistoryStoreService operationPlanHistoryStoreService,
            IHistoryClinicReportStoreService historyClinicReportStoreService,
            IOvertimeUtilizationReportStoreService overtimeUtilizationStoreService
            )
        {
            _overtimeReportStoreService = overtimeReportStoreService;
            _operationPlanHistoryStoreService = operationPlanHistoryStoreService;
            _historyClinicReportStoreService = historyClinicReportStoreService;
            _overtimeUtilizationStoreService = overtimeUtilizationStoreService;
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

            var items = await _overtimeReportStoreService.GetExportAsync(input);

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(OvertimeReportExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [Route("OperationPlan/GetOperationPlanHistory")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetOperationPlanHistory(HistoryPlanningInputModel input)
        {
            return await _operationPlanHistoryStoreService.GetAsync<OperationPlanHistoryOutputModel>(input);
        }

        [Route("OperationPlan/GetOperationListByDate")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetOperationListByDate(GridInputModel input, DateTime operationDate)
        {
            return await _operationPlanHistoryStoreService.GetOperationListByDate(input, operationDate);
        }

        [Route("Report/HistoryPlanningReportExcelExport")]
        public async Task<string> HistoryPlanningReportExcelExport(HistoryPlanningInputModel input)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("History_Plannings_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operationPlanHistoryStoreService.GetExportAsync<HistoryPlanningReportExportModel>(input);

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(HistoryPlanningReportExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [HttpGet]
        [Route("Report/GetClinicHistory")]
        public async Task<ResultModel<HistoryClinicReportOutputModel>> HistoryClinicReportPage(HistoryClinicReportInputModel input)
        {
            var result = await _historyClinicReportStoreService.GetAsync<HistoryClinicReportOutputModel>(input);

            return result;
        }

        [Route("Report/HistoryClinicReportExcelExport")]
        public async Task<string> HistoryClinicReportExcelExport(HistoryClinicReportInputModel input)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("History_Clinics_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _historyClinicReportStoreService.GetExportAsync<HistoryClinicReportExportModel>(input);

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(HistoryClinicReportExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [HttpGet]
        [Route("Report/GetOvertimeUtilization")]
        public async Task<ResultModel<OvertimeUtilizationReportOutputModel>> GetOvertimeUtilization(OvertimeUtilizationReportInputModel input)
        {
            return await _overtimeUtilizationStoreService.GetAsync<OvertimeUtilizationReportOutputModel>(input);
        }

        [Route("Report/OvertimeUtilizationReportExcelExport")]
        public async Task<string> OvertimeUtilizationReportExcelExport(OvertimeUtilizationReportInputModel input)
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Overtime_Utilization_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _overtimeUtilizationStoreService.GetExportAsync(input);

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(OvertimeUtilizationReportExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }
    }
}