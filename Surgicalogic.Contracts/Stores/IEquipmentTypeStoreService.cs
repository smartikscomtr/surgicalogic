using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IEquipmentTypeStoreService : IStoreService<EquipmentTypeModel, EquipmentTypeSorting, EquipmentTypeFilter>
    {
    }
    public enum EquipmentTypeSorting
    {
        NotSet,

        NameAsc,
        NameDesc
    }
    public enum EquipmentTypeFilter
    {
        NotSet,
        Id,
        Name
    }

}
