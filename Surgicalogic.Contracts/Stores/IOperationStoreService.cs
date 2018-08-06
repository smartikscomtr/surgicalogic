using System.Collections.Generic;
using System.Threading.Tasks;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Planning.Model.InputModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationStoreService : IStoreService<Operation, OperationModel>
    {
        Task<List<OperationInputModel>> GetTomorrowOperationsAsync();
    }
}