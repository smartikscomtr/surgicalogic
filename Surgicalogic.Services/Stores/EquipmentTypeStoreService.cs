using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.QueryBuilder;
using Surgicalogic.Services.QueryBuilder.Enums;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;

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
