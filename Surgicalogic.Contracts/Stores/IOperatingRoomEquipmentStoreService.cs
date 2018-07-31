using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomEquipmentStoreService : IStoreService<OperatingRoomEquipment, OperatingRoomEquipmentModel>
    {
        Task<List<OperatingRoomEquipmentModel>> GetByOperatingRoomIdAsync(int operatingRoomId);
        Task<bool> CheckEquipmentsRelatedToOperationRoom(int[] equipmentIds);
        Task<List<OperatingRoomEquipmentModel>> GetByEquipmentIdAsync(int equipmentId);
        Task<ResultModel<EquipmentOutputModel>> UpdateEquipmentOperatingRoomsAsync(EquipmentInputModel item);
        Task DeleteByOperatingRoomIdAsync(int id);
    }
}