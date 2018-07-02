using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IPersonnelTitleStoreService : IStoreService<PersonnelTitleModel, PersonnelTitleSorting, PersonnelTitleFilter>
    {
    }

    public enum PersonnelTitleSorting
    {
        NameAsc,
        NameDesc,

        DescriptionAsc,
        DescriptionDesc
    }

    public enum PersonnelTitleFilter
    {
        Id,
        Name,
        Description
    }
}
