﻿using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.Stores
{
    public class OperationTypeStoreService : StoreService<OperationType, OperationTypeModel>, IOperationTypeStoreService
    {
        public OperationTypeStoreService(DataContext context) : base(context)
        {
        }
    }
}
