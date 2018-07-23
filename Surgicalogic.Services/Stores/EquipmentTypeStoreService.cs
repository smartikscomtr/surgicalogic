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
    }
}