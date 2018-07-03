﻿using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentTypeStoreService : StoreService<EquipmentType, EquipmentTypeModel, EquipmentTypeSorting, EquipmentTypeFilter>, IEquipmentTypeStoreService
    {
        public EquipmentTypeStoreService(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
