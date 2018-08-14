using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelStoreService : StoreService<Personnel, PersonnelModel>, IPersonnelStoreService
    {
        private DataContext _context;
        public PersonnelStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PersonnelOutputModel>> GetDoctorsByBranchIdAsync(int branchId)
        {
            return await GetQueryable().Where(x => x.PersonnelBranches.Any(t => t.BranchId == branchId && x.PersonnelTitleId == AppSettings.DoctorId && t.IsActive)).ProjectTo<PersonnelOutputModel>().ToListAsync();
        }
    }
}