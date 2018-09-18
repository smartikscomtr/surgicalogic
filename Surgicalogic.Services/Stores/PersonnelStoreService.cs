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

        public async Task<List<PersonnelOutputModel>> GetPersonnelsByBranchIdAsync(int branchId)
        {
            return await GetQueryable().Where(x => x.PersonnelBranches.Any(t => t.BranchId == branchId && t.IsActive)).ProjectTo<PersonnelOutputModel>().ToListAsync();
        }

        public async Task<List<PersonnelOutputModel>> GetDoctorsByBranchIdAsync(int branchId)
        {
            var query = GetQueryable().Where(x => x.PersonnelBranches.Any(t => t.IsActive && t.Personnel.PersonnelTitleId == AppSettings.DoctorId));

            if(branchId > 0)
            { 
                query = query.Where(x => x.PersonnelBranches.Any(y => y.BranchId == branchId));
            }

            return await query.ProjectTo<PersonnelOutputModel>().ToListAsync();
        }
    }
}