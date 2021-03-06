﻿using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperatingRoomCalendarStoreService : IStoreService<OperatingRoomCalendar, OperatingRoomCalendarModel>
    {
        Task<ResultModel<OperatingRoomCalendarOutputModel>> GetByOperatingRoomIdAsync(int operatingRoomId);
        Task<List<OperationRoomCalendarExportModel>> GetExportAsync<OperationRoomCalendarExportModel>(int id);
    }
}
