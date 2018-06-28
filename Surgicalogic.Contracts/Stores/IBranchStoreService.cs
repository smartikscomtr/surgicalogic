using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IBranchStoreService : IStoreService<BranchModel, BranchSorting, BranchFilter>
    {
    }

    public enum BranchSorting
    {
        NotSet,

        NameAsc,
        NameDesc,

        DescriptionAsc,
        DescriptionDesc
    }

    public enum BranchFilter
    {
        NotSet,
        Id,
        Name,
        Description
    }
}
