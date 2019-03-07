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
    }
}
