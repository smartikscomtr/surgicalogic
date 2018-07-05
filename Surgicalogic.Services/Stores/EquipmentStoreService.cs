using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentStoreService : StoreService<Equipment, EquipmentModel>, IEquipmentStoreService
    {
        private DataContext _context;
        public EquipmentStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        //public override IQueryable<Equipment> GetQueryable()
        //{
        //       return _context.Set<Equipment>().AsNoTracking().Include(e => e.EquipmentType);
        //}

    }
}
