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
    public class EquipmentStoreService : StoreService<Equipment, EquipmentModel, EquipmentSorting, EquipmentFilter>, IEquipmentStoreService 
    {
        public EquipmentStoreService(IConfiguration configuration) 
            : base(configuration)
        {
        }

        protected override Task SetSortingAsync(SelectQueryBuilder query, EquipmentSorting? sorting)
        {
            if(sorting.HasValue)
            {
                switch (sorting.Value)
                {
                    case EquipmentSorting.NameAsc:
                        query.OrderClause.AddStatement("main", "Name");
                        break;

                    case EquipmentSorting.NameDesc:
                        query.OrderClause.AddStatement("main", "Name", Sorting.Descending);
                        break;


                    case EquipmentSorting.IsPortableAsc:
                        query.OrderClause.AddStatement("main", "IsHot");
                        break;

                    case EquipmentSorting.IsPortableDesc:
                        query.OrderClause.AddStatement("main", "IsHot", Sorting.Descending);
                        break;                        
                }
            }

            return Task.CompletedTask;
        }
    }
}
