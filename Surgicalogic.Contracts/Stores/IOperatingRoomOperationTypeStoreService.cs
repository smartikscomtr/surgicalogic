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
    public interface IOperatingRoomOperationTypeStoreService : IStoreService<OperatingRoomOperationType, OperatingRoomOperationTypeModel>
    {
        Task<List<OperatingRoomOperationTypeModel>> GetByOperatingRoomIdAsync(int operatingRoomId);
        Task<ResultModel<OperationTypeOutputModel>> UpdateOperationTypeOperatingRoomsAsync(OperationTypeInputModel item);

    }

}
