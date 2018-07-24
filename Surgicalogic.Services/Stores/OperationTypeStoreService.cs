﻿using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class OperationTypeStoreService : StoreService<OperationType, OperationTypeModel>, IOperationTypeStoreService
    {
        private DataContext _context;
        public OperationTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}