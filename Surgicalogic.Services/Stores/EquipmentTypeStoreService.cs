using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentTypeStoreService : StoreService<EquipmentType, EquipmentTypeModel>, IEquipmentTypeStoreService
    {
        private DataContext _context;
        public EquipmentTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        //public override async Task<ResultModel<int>> DeleteByIdAsync(int id)
        //{
        //    var info = new Info();
        //    var hasEquipment = await _context.Set<Equipment>().Where(e => e.EquipmentTypeId == id).CountAsync() > 0;            

        //    if(hasEquipment)
        //    {
        //        info.Message = "Bu tipe bağlı ekipman olduğundan silinememektedir.";
        //        info.Succeeded = false;
        //    }
        //    else
        //    {
        //        var entity = await _context.Set<EquipmentType>().FirstAsync(e => e.Id == id);

        //        entity.IsActive = false;
        //        entity.ModifiedBy = 0;
        //        entity.ModifiedDate = DateTime.Now;

        //        await _context.SaveChangesAsync();
        //    }
            

        //    return new ResultModel<int>
        //    {
        //        Result = null,
        //        Info = info
        //    };
        //}

    }
}