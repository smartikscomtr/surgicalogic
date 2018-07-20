using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomEquipmentStoreService : IStoreService<OperatingRoomEquipment, OperatingRoomEquipmentModel>
    {
        Task<List<OperatingRoomEquipmentModel>> GetByOperatingRoomIdAsync(int operatingRoomId);
        Task<bool> CheckEquipmentsRelatedOperationRoom(int[] equipmentIds);
    }

}
