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

        public async Task<bool> CheckEquipmentsRelatedOperationRoom(int[] equipmentIds)
        {
            return await GetQueryable().AnyAsync(x => equipmentIds.Contains(x.EquipmentId));
        }
    }
}
