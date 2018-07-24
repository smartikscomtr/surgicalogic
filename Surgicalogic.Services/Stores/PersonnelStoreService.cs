﻿using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelStoreService : StoreService<Personnel, PersonnelModel>, IPersonnelStoreService
    {
        private DataContext _context;
        public PersonnelStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}