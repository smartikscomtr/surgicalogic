using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomOperationTypeStoreService : IStoreService<OperatingRoomOperationType, OperatingRoomOperationTypeModel>
    {
        Task<List<OperatingRoomOperationTypeModel>> GetByOperatingRoomIdAsync(int operatingRoomId);
    }

}
