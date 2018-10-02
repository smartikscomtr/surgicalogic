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
        private IOperationTypeStoreService _operationTypeStoreService;
        public PersonnelStoreService(DataContext context, IOperationTypeStoreService operationTypeStoreService) : base(context)
        {
            _context = context;
            _operationTypeStoreService = operationTypeStoreService;
        }

        public async Task<List<PersonnelOutputModel>> GetPersonnelsByBranchIdAsync(int branchId)
        {
            return await GetQueryable().Where(x => x.PersonnelBranches.Any(t => t.BranchId == branchId && t.IsActive)).ProjectTo<PersonnelOutputModel>().ToListAsync();
        }

        public async Task<List<PersonnelOutputModel>> GetPersonnelsByOperationTypeIdAsync(int operationTypeId)
        {
            var operationType = await _operationTypeStoreService.FindByIdAsync(operationTypeId);

            return await GetPersonnelsByBranchIdAsync(operationType.Result.BranchId);
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