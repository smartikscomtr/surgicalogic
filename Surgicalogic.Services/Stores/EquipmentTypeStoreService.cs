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

        public override async Task<ResultModel<int>> DeleteAndSaveByIdAsync(int id)
        {
            if (_context.Equipments.Any(x => x.EquipmentTypeId == id && x.IsActive))
            {
                return new ResultModel<int> {
                  Info = new Info
                  {
                      InfoType = Model.Enum.InfoType.Error,
                      Message = Model.Enum.MessageType.ModelHasRelationalData
                  }  
                };
            }

            return await base.DeleteAndSaveByIdAsync(id);
        }
    }
}