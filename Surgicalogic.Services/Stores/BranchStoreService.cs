using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Services.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Contracts.Stores;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Surgicalogic.Services.QueryBuilder;
using Surgicalogic.Services.QueryBuilder.Enums;

namespace Surgicalogic.Services.Stores
{
    public class BranchStoreService : StoreService<Branch, BranchModel, BranchSorting, BranchFilter>, IBranchStoreService
    {
        public BranchStoreService(IConfiguration configuration) : base(configuration)
        {
        }

        protected override Task SetSortingAsync(SelectQueryBuilder query, BranchSorting? sorting)
        {
            if(sorting.HasValue)
            {
                switch(sorting.Value)
                {
                    case BranchSorting.NameAsc:
                        query.OrderClause.AddStatement("main", "Name");
                        break;

                    case BranchSorting.NameDesc:
                        query.OrderClause.AddStatement("main", "name", Sorting.Descending);
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
