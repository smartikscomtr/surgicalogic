using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Contracts.StoreServices.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.StoreServices
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
