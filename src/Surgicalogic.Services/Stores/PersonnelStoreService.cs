﻿using AutoMapper.QueryableExtensions;
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
            var query = GetQueryable().Where(x => x.PersonnelBranches.Any(t => t.IsActive && t.Personnel.PersonnelCategoryId == AppSettings.DoctorId));

            if(branchId > 0)
            { 
                query = query.Where(x => x.PersonnelBranches.Any(y => y.BranchId == branchId));
            }

            return await query.ProjectTo<PersonnelOutputModel>().ToListAsync();
        }

        public async Task<PersonnelOutputModel> GetPersonnelByIdAsync(int id)
        {
            return await  GetQueryable().Where(x => x.Id == id).ProjectTo<PersonnelOutputModel>().FirstOrDefaultAsync();
        }

        public async Task UpdatePhotoAsync(int id, string fileName)
        {
            var entity = GetQueryable().ProjectTo<PersonnelModel>().SingleOrDefault(x => x.Id == id);
            entity.PictureUrl = fileName;
            await UpdateAndSaveAsync(entity);
        }

        public async Task<bool> IsDuplicateCode(string personnelCode, int id)
        {
            return await GetQueryable().AnyAsync(x => x.PersonnelCode == personnelCode && x.Id != id);
        }
    }
}