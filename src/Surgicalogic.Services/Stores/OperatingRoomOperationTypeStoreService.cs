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

        public async Task<ResultModel<OperationTypeOutputModel>> UpdateOperationTypeOperatingRoomsAsync(OperationTypeInputModel item)
        {
            var result = new ResultModel<OperationTypeOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentOperatingRooms = await GetByOperationTypeIdAsync(item.Id);
            var operatingRoomIds = currentOperatingRooms.Select(x => x.OperatingRoomId);
            var addedOperationRooms = item.OperatingRoomIds.Except(operatingRoomIds);
            var removedOperationRooms = operatingRoomIds.Except(item.OperatingRoomIds);

            foreach (var operationRoomId in addedOperationRooms)
            {
                await InsertAsync(new OperatingRoomOperationTypeModel
                {
                    OperatingRoomId = operationRoomId,
                    OperationTypeId = item.Id
                });
            }

            foreach (var operationRoomId in removedOperationRooms)
            {
                await DeleteByIdAsync(currentOperatingRooms.First(x => x.OperationTypeId == item.Id && x.OperatingRoomId == operationRoomId).Id);
            }

            await SaveChangesAsync();

            return result;
        }

        private async Task<List<OperatingRoomOperationTypeModel>> GetByOperationTypeIdAsync(int operationTypeId)
        {
            return await GetQueryable().Where(x => x.OperationTypeId == operationTypeId).ProjectTo<OperatingRoomOperationTypeModel>().ToListAsync();
        }
    }
}
