using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores.IReportStoreService
{
    public interface IHistoryClinicReportStoreService
    {
        Task<ResultModel<HistoryClinicReportOutputModel>> GetAsync<TOutputModel>(HistoryClinicReportInputModel input);
        Task<List<HistoryClinicReportExportModel>> GetExportAsync<HistoryClinicReportOutputModel>(HistoryClinicReportInputModel input);
    }
}
