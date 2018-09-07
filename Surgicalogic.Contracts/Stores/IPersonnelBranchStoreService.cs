using System.Collections.Generic;
using System.Threading.Tasks;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IPersonnelBranchStoreService : IStoreService<PersonnelBranch, PersonnelBranchModel>
    {
        Task<ResultModel<PersonnelOutputModel>> UpdatePersonelBranchAsync(int personnelId, int[] branchId);
        Task<List<int>> GetPersonnelIdsByBranchIdAsync(int branchId);
    }
}