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
    public class OperatingRoomEquipmentStoreService : StoreService<OperatingRoomEquipment, OperatingRoomEquipmentModel>, IOperatingRoomEquipmentStoreService
    {
        private DataContext _context;
        public OperatingRoomEquipmentStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OperatingRoomEquipmentModel>> GetByOperatingRoomIdAsync(int operatingRoomId)
        {
           return await GetQueryable().Where(x => x.OperatingRoomId == operatingRoomId).ProjectTo<OperatingRoomEquipmentModel>().ToListAsync();
        }

        public async Task<List<OperatingRoomEquipmentModel>> GetByEquipmentIdAsync(int equipmentId)
        {
            return await GetQueryable().Where(x => x.EquipmentId == equipmentId).ProjectTo<OperatingRoomEquipmentModel>().ToListAsync();
        }

        public async Task<bool> CheckEquipmentsRelatedToOperationRoom(int[] equipmentIds)
        {
            return await GetQueryable().AnyAsync(x => equipmentIds.Contains(x.EquipmentId));
        }

        public async Task<ResultModel<EquipmentOutputModel>> UpdateEquipmentOperatingRoomsAsync(EquipmentInputModel item)
        {
            var result = new ResultModel<EquipmentOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentOperatingRooms = await GetByEquipmentIdAsync(item.Id);
            var operatingRoomIds = currentOperatingRooms.Select(x => x.OperatingRoomId);
            var added = item.OperatingRoomIds.Except(operatingRoomIds);
            var removed = operatingRoomIds.Except(item.OperatingRoomIds);

            foreach (var add in added)
            {
                await InsertAsync(new OperatingRoomEquipmentModel
                {
                    OperatingRoomId = add,
                    EquipmentId = item.Id
                });
            }

            foreach (var remove in removed)
            {
                await DeleteByIdAsync(currentOperatingRooms.First(x => x.EquipmentId == item.Id && x.OperatingRoomId == remove).Id);
            }

            await SaveChangesAsync();

            return result;
        }

        public async Task DeleteByOperatingRoomIdAsync(int id)
        {
            var results = await GetByOperatingRoomIdAsync(id);

            foreach (var result in results)
            {
                await DeleteByIdAsync(result.Id);
            }

            await SaveChangesAsync();
        }
    }
}
