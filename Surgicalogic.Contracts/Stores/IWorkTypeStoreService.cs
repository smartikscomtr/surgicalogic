using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IWorkTypeStoreService : IStoreService<WorkTypeModel, WorkTypeSorting, WorkTypeFilter>
    {
    }

    public enum WorkTypeSorting
    {
        NotSet,

        NameAsc,
        NameDesc,

        DescriptionAsc,
        DescriptionDesc
    }

    public enum WorkTypeFilter
    {
        Id,
        Name,
        Description
    }
}
