using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Services.Stores
{
    public class PersonnelBranchStoreService : StoreService<PersonnelBranch, PersonnelBranchModel>, IPersonnelBranchStoreService
    {
        private DataContext _context;
        public PersonnelBranchStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel<PersonnelOutputModel>> UpdatePersonelBranchAsync(int personnelId, int[] branchIds)
        {
            var result = new ResultModel<PersonnelOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentBranches = await GetCurrentBranchesByPersonnelIdAsync(personnelId);
            var currentbranchIds = currentBranches.Select(x => x.BranchId);
            var addedBranches = branchIds.Except(currentbranchIds);
            var removedBranches = currentbranchIds.Except(branchIds);

            foreach (var branchId in addedBranches)
            {
                await InsertAsync(new PersonnelBranchModel
                {
                    PersonnelId = personnelId,
                    BranchId = branchId
                });
            }

            foreach (var branchId in removedBranches)
            {
                await DeleteByIdAsync(currentBranches.First(x => x.PersonnelId == personnelId && x.BranchId == branchId).Id);
            }

            await SaveChangesAsync();

            return result;
        }

        public async Task<List<int>> GetPersonnelIdsByBranchIdAsync(int branchId)
        {
            return await GetQueryable().Where(x => x.BranchId == branchId).Select(x => x.PersonnelId).ToListAsync();
        }

        private async Task<List<PersonnelBranchModel>> GetCurrentBranchesByPersonnelIdAsync(int personnelId)
        {
            return await GetQueryable().Where(x => x.PersonnelId == personnelId).ProjectTo<PersonnelBranchModel>().ToListAsync();
        }
    }
}