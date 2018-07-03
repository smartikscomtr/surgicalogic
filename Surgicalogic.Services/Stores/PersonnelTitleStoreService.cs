using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelTitleStoreService : StoreService<PersonnelTitle, PersonnelTitleModel>, IPersonnelTitleStoreService
    {
        public PersonnelTitleStoreService(DataContext context) : base(context)
        {
        }


    }
}
