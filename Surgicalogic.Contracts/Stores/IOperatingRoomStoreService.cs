﻿using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomStoreService : IStoreService<OperatingRoom, OperatingRoomModel>
    {
    }
}