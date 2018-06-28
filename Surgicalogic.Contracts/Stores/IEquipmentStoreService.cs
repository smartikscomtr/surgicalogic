using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IEquipmentStoreService : IStoreService<EquipmentModel, EquipmentSorting, EquipmentFilter>
    {
    }
    public enum EquipmentSorting
    {
        NotSet,

        NameAsc,
        NameDesc,

        DescriptionAsc,
        DescriptionDesc,

        IsPortableAsc,
        IsPortableDesc
    }
    public enum EquipmentFilter
    {
        NotSet,
        Id,
        Name,
        Description,
        IsPortable
    }
}
