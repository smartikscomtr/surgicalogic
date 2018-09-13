using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.Stores
{
    public class PatientStoreService : StoreService<Patient, PatientModel>, IPatientStoreService
    {
        private DataContext _context;

        public PatientStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
