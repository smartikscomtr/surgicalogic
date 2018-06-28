﻿using Microsoft.Extensions.Configuration;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.QueryBuilder;
using Surgicalogic.Services.QueryBuilder.Enums;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentTypeStoreService : StoreService<EquipmentType, EquipmentTypeModel, EquipmentTypeSorting, EquipmentTypeFilter>, IEquipmentTypeStoreService
    {
        public EquipmentTypeStoreService(IConfiguration configuration)
            : base(configuration)
        {

        }
        protected override Task SetSortingAsync(SelectQueryBuilder query, EquipmentTypeSorting? sorting)
        {
            if (sorting.HasValue)
            {
                switch (sorting.Value)
                {
                    case EquipmentTypeSorting.NameAsc:
                        query.OrderClause.AddStatement("main", "Name");
                        break;

                    case EquipmentTypeSorting.NameDesc:
                        query.OrderClause.AddStatement("main", "Name", Sorting.Descending);
                        break;
                }
            }

            return Task.CompletedTask;
        }


    }
}
