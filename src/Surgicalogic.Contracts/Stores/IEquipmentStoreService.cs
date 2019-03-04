using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IEquipmentStoreService : IStoreService<Equipment, EquipmentModel>
    {
        Task<ResultModel<EquipmentOutputModel>> GetNonPortableEquipments();
        Task<bool> IsDuplicateCode(string code, int id);
    }
}