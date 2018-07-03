using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IPersonnelStoreService : IStoreService<PersonnelModel, PersonnelSorting, PersonnelFilter>
    {
    }

    public enum PersonnelSorting
    {
        NotSet,

        FirstNameAsc,
        FirstNameDesc,

        LastNameAsc,
        LastNameDesc
    }

    public enum PersonnelFilter
    {
        NotSet,
        Id,
        PersonnelCode,
        FirstName,
        LastName,
        Branch,
        WorkType
    }
}
