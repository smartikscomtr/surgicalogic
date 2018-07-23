using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;

namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomOperationTypeStoreService : StoreService<OperatingRoomOperationType, OperatingRoomOperationTypeModel>, IOperatingRoomOperationTypeStoreService
    {
        private DataContext _context;
        public OperatingRoomOperationTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OperatingRoomOperationTypeModel>> GetByOperatingRoomIdAsync(int operatingRoomId)
        {
           return await GetQueryable().Where(x => x.OperatingRoomId == operatingRoomId).ProjectTo<OperatingRoomOperationTypeModel>().ToListAsync();
        }
    }
}
