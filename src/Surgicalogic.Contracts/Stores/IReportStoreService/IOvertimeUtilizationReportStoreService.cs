using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.ExportModel.Report;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores.IReportStoreService
{
    public interface IOvertimeUtilizationReportStoreService
    {
        Task<ResultModel<OvertimeUtilizationReportOutputModel>> GetAsync<TOutputModel>(OvertimeUtilizationReportInputModel input);
        Task<List<OvertimeUtilizationForOvertimeReportOutputModel>> GetExportAsync<OvertimeUtilizationReportOutputModel>(OvertimeUtilizationReportInputModel input);
    }
}
