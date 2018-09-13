using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationPlanHistoryStoreService : IStoreService<OperationPlan, OperationPlanHistoryModel>
    {
        Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationListAsync(GridInputModel input);
    }
}