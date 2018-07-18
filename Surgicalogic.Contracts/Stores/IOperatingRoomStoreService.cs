using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomStoreService : IStoreService<OperatingRoom, OperatingRoomModel>
    {
        Task<ResultModel<OperatingRoomModel>> UpdateAndSaveOperatingRoomAsync(OperatingRoomInputModel model);
    }
}