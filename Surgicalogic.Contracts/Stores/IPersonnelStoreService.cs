using System.Collections.Generic;
using System.Threading.Tasks;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IPersonnelStoreService : IStoreService<Personnel, PersonnelModel>
    {
        Task<List<PersonnelOutputModel>> GetDoctorsByBranchIdAsync(int branchId);
    }
}