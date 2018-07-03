using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelStoreService : StoreService<Personnel, PersonnelModel, PersonnelSorting, PersonnelFilter>, IPersonnelStoreService
    {
        public PersonnelStoreService(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
