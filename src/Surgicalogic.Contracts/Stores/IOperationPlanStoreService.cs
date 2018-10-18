using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationPlanStoreService : IStoreService<OperationPlan, OperationPlanModel>
    {
        Task<List<OperationPlanOutputModel>> GetDashboardTimelineOperationsAsync(DateTime selectDate);
        Task DeleteTomorrowPlanAsync();
        Task<List<OperationPlanModel>> GetByIdListAsync(int[] updatedItemIds);
    }
}