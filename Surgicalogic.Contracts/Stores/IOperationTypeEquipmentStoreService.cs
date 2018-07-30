using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationTypeEquipmentStoreService : IStoreService<OperationTypeEquipment, OperationTypeEquipmentModel>
    {
    }
}
