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
        public OperationPersonnelStoreService(DataContext context) : base(context)
        {
            _context = context;
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

            var currentOperations = await GetByOperationIdAsync(item.Id);
            var doctors = currentOperations.Select(x => x.PersonnelId);
            var addedDoctors = item.DoctorIds.Except(doctors);
            var removedDoctors = doctors.Except(item.DoctorIds);

            foreach (var doctorId in addedDoctors)
            {
                await InsertAsync(new OperationPersonnelModel
                {
                    PersonnelId = doctorId,
                    OperationId = item.Id
                });
            }

            foreach (var doctorId in removedDoctors)
            {
                await DeleteByIdAsync(currentOperations.First(x => x.OperationId == item.Id && x.PersonnelId == doctorId).Id);
            }

            await SaveChangesAsync();

            return result;
        }
    }
}
