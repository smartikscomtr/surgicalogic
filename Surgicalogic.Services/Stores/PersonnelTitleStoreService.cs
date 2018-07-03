using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelTitleStoreService : StoreService<PersonnelTitle, PersonnelTitleModel, PersonnelTitleSorting, PersonnelTitleFilter>, IPersonnelTitleStoreService
    {
        public PersonnelTitleStoreService(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
