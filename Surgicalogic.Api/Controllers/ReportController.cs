using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}