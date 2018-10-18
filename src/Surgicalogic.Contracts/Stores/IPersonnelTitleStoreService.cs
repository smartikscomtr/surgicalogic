using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IPersonnelTitleStoreService : IStoreService<PersonnelTitle, PersonnelTitleModel>
    {
    }
}