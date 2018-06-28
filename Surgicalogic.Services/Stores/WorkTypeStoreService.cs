using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.QueryBuilder;
using Surgicalogic.Services.QueryBuilder.Enums;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class WorkTypeStoreService : StoreService<WorkType, WorkTypeModel, WorkTypeSorting, WorkTypeFilter>, IWorkTypeStoreService
    {
        public WorkTypeStoreService(IConfiguration configuration) : base(configuration)
        {
        }

        protected override Task SetSortingAsync(SelectQueryBuilder query, WorkTypeSorting? sorting)
        {
            if (sorting.HasValue)
            {
                switch (sorting.Value)
                {
                    case WorkTypeSorting.NameAsc:
                        query.OrderClause.AddStatement("main", "Name");
                        break;

                    case WorkTypeSorting.NameDesc:
                        query.OrderClause.AddStatement("main", "name", Sorting.Descending);
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
