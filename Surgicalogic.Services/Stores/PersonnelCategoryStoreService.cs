using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelCategoryStoreService : StoreService<PersonnelCategory, PersonnelCategoryModel>, IPersonnelCategoryStoreService
    {
        private DataContext _context;
        public PersonnelCategoryStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}