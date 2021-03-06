﻿using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.ExportModel.Report;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationPlanHistoryStoreService
    {
        Task<ResultModel<OperationPlanListOutputModel>> GetOperationListByDate(GridInputModel input, DateTime operationDate);
        Task<ResultModel<OperationPlanHistoryOutputModel>> GetAsync<TOutputModel>(HistoryPlanningInputModel input);
        Task<List<HistoryPlanningReportExportModel>> GetExportAsync<OperationPlanHistoryOutputModel>(HistoryPlanningInputModel input);

    }
}