using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelTitleStoreService : StoreService<PersonnelTitle, PersonnelTitleModel>, IPersonnelTitleStoreService
    {
        private DataContext _context;
        public PersonnelTitleStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}