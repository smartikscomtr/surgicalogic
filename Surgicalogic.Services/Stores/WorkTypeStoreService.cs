using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class WorkTypeStoreService : StoreService<WorkType, WorkTypeModel>, IWorkTypeStoreService
    {
        public WorkTypeStoreService(DataContext context) : base(context)
        {
        }

    }
}
