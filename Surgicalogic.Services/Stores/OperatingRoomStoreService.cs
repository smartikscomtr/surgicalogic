using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomStoreService : StoreService<OperatingRoom, OperatingRoomModel>, IOperatingRoomStoreService
    {
        private DataContext _context;
        public OperatingRoomStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel<OperatingRoomModel>> UpdateOperatingRoomEquipmentsAsync(OperatingRoomInputModel item)
        {   
            #region OperatingRommEquipments entity update
            
            #endregion
            
            return new ResultModel<OperatingRoomModel>
            {
                Result = item,
                Info = new Info()
            };

            
        }


    }
}