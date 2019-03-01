using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomStoreService : IStoreService<OperatingRoom, OperatingRoomModel>
    {
        Task<ResultModel<OperatingRoomOutputModel>> UpdateOperatingRoomEquipmentsAsync(OperatingRoomInputModel item);
        Task<ResultModel<OperatingRoomOutputModel>> UpdateOperatingRoomOperationTypesAsync(OperatingRoomInputModel item);
        Task<List<RoomInputModel>> GetAvailableRoomsAsync(DateTime date);
        Task<List<OperatingRoomOutputModel>> GetByOperationTypeIdAsync(int operationTypeId);
        Task<List<RoomInputModel>> GetOperatingRoomsForDashboardTimelineModelAsync(DateTime selectDate, bool activeOnly = true);
    }
}