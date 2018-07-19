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

        public async Task<ResultModel<OperatingRoomModel>> UpdateAndSaveOperatingRoomAsync(OperatingRoomInputModel item)
        {
            var model = new OperatingRoomModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length
            };

            #region OperatingRoom entity update

            var entity = await _context.Set<OperatingRoom>().FirstAsync(e => e.Id == model.Id);

            Mapper.Map(model, entity);

            entity.ModifiedBy = 2;

            entity.ModifiedDate = DateTime.Now;

            #endregion

            #region OperatingRommEquipments entity update
            
            if(item.Equipments.Count > 0)
            {

            }

            #endregion

            await _context.SaveChangesAsync();

            return new ResultModel<OperatingRoomModel>
            {
                Result = model,
                Info = new Info()
            };

            
        }


    }
}