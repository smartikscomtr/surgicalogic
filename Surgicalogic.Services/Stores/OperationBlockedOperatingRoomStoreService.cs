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
    public class OperationBlockedOperatingRoomStoreService : StoreService<OperationBlockedOperatingRoom, OperationBlockedOperatingRoomModel>, IOperationBlockedOperatingRoomStoreService
    {
        private DataContext _context;
        public OperationBlockedOperatingRoomStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OperationBlockedOperatingRoomModel>> GetByOperationIdAsync(int operationId)
        {
           return await GetQueryable().Where(x => x.OperationId == operationId).ProjectTo<OperationBlockedOperatingRoomModel>().ToListAsync();
        }

        public async Task<ResultModel<OperationOutputModel>> UpdateOperatingRoomsAsync(OperationInputModel item)
        {
            var result = new ResultModel<OperationOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentOperations = await GetByOperationIdAsync(item.Id);
            var blockedRooms = currentOperations.Select(x => x.OperatingRoomId);
            var addedRooms = item.OperatingRoomIds.Except(blockedRooms);
            var removedRooms = blockedRooms.Except(item.OperatingRoomIds);

            foreach (var roomId in addedRooms)
            {
                await InsertAsync(new OperationBlockedOperatingRoomModel
                {
                    OperatingRoomId = roomId,
                    OperationId = item.Id
                });
            }

            foreach (var roomId in removedRooms)
            {
                await DeleteByIdAsync(currentOperations.First(x => x.OperationId == item.Id && x.OperatingRoomId == roomId).Id);
            }

            await SaveChangesAsync();

            return result;
        }
    }
}
