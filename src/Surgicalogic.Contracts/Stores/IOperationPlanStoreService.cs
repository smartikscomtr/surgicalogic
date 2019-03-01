using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
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
        Task DeletePlanByDateAsync(DateTime date);
        Task<List<OperationPlanModel>> GetByIdListAsync(int[] updatedItemIds);
        Task<List<SimulationOperationPlanModel>> GetOperationByIdListAsync(DateTime selectDate);
    }
}