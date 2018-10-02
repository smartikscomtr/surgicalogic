using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class OperationPersonnelStoreService : StoreService<OperationPersonnel, OperationPersonnelModel>, IOperationPersonnelStoreService
    {
        private DataContext _context;
        private IPersonnelStoreService _personnelStoreService;
        public OperationPersonnelStoreService(DataContext context, IPersonnelStoreService personnelStoreService) : base(context)
        {
            _context = context;
            _personnelStoreService = personnelStoreService;
        }

        public async Task<List<OperationPersonnelModel>> GetByOperationIdAsync(int operationId)
        {
           return await GetQueryable().Where(x => x.OperationId == operationId).ProjectTo<OperationPersonnelModel>().ToListAsync();
        }

        public async Task<ResultModel<OperationOutputModel>> UpdateOperationPersonnelsAsync(OperationInputModel item)
        {
            var result = new ResultModel<OperationOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentPersonnels = await GetByOperationIdAsync(item.Id);
            var personnels = currentPersonnels.Select(x => x.PersonnelId);
            var branchPersonnels = await _personnelStoreService.GetPersonnelsByOperationTypeIdAsync(item.OperationTypeId);
            var addedPersonnels = item.PersonnelIds.Except(personnels);

            addedPersonnels = branchPersonnels.Select(x => x.Id).Intersect(addedPersonnels);
            var removedPersonnels = personnels.Except(item.PersonnelIds);

            foreach (var personnelId in addedPersonnels)
            {
                await InsertAsync(new OperationPersonnelModel
                {
                    PersonnelId = personnelId,
                    OperationId = item.Id
                });
            }

            foreach (var personnelId in removedPersonnels)
            {
                await DeleteByIdAsync(currentPersonnels.First(x => x.OperationId == item.Id && x.PersonnelId == personnelId).Id);
            }

            await SaveChangesAsync();

            return result;
        }
    }
}
