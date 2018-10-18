using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores.IReportStoreService
{
    public interface IOvertimeReportStoreService
    {
        Task<ResultModel<OvertimeReportOutputModel>> GetAsync<TOutputModel>(OvertimeReportInputModel input);
    }
}
