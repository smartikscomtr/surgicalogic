﻿using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationBlockedOperatingRoomStoreService : IStoreService<OperationBlockedOperatingRoom, OperationBlockedOperatingRoomModel>
    {
        Task<ResultModel<OperationOutputModel>> UpdateOperatingRoomsAsync(OperationInputModel item);
    }
}
