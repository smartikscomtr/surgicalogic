using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomStoreService : StoreService<OperatingRoom, OperatingRoomModel>, IOperatingRoomStoreService
    {
        private DataContext _context;
        private IOperatingRoomEquipmentStoreService _operatingRoomEquipmentStoreService;
        private IOperatingRoomOperationTypeStoreService _operatingRoomOperationTypeStoreService;

        public OperatingRoomStoreService(
            DataContext context,
            IOperatingRoomEquipmentStoreService operatingRoomEquipmentStoreService,
            IOperatingRoomOperationTypeStoreService operatingRoomOperationTypeStoreService
            ) : base(context)
        {
            _context = context;
            _operatingRoomEquipmentStoreService = operatingRoomEquipmentStoreService;
            _operatingRoomOperationTypeStoreService = operatingRoomOperationTypeStoreService;
        }

        public async Task<List<RoomInputModel>> GetAvailableRoomsAsync()
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            return await _context.OperatingRooms.Where(x => x.IsActive && x.IsAvailable && !x.OperatingRoomCalendars.Any(t => t.StartDate <= tomorrow && t.EndDate >= tomorrow && t.IsActive)).ProjectTo<RoomInputModel>().ToListAsync();
        }

        public async Task<ResultModel<OperatingRoomOutputModel>> UpdateOperatingRoomEquipmentsAsync(OperatingRoomInputModel item)
        {
            var result = new ResultModel<OperatingRoomOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentEquipments = await _operatingRoomEquipmentStoreService.GetByOperatingRoomIdAsync(item.Id);
            var equipmentIds = currentEquipments.Select(x => x.EquipmentId);
            var addedEquipments = item.Equipments.Except(equipmentIds);
            var removedEquipments = equipmentIds.Except(item.Equipments);

            //bool isEquipmentRelatedToOperatingRoom = await _operatingRoomEquipmentStoreService.CheckEquipmentsRelatedToOperationRoom(addedEquipments.ToArray());

            //if (isEquipmentRelatedToOperatingRoom)
            //{
            //    result.Info = new Info
            //    {
            //        Succeeded = false,
            //        InfoType = Model.Enum.InfoType.Error,
            //        Message = Model.Enum.MessageType.EquipmentRelatedToOperatingRoom
            //    };

            //    return result;
            //}

            foreach (var equipmentId in addedEquipments)
            {
                await _operatingRoomEquipmentStoreService.InsertAsync(new OperatingRoomEquipmentModel
                {
                    EquipmentId = equipmentId,
                    OperatingRoomId = item.Id
                });
            }

            foreach (var equipment in removedEquipments)
            {
                await _operatingRoomEquipmentStoreService.DeleteByIdAsync(currentEquipments.First(x => x.OperatingRoomId == item.Id && x.EquipmentId == equipment).Id);
            }

            await _operatingRoomEquipmentStoreService.SaveChangesAsync();

            return result;
        }

        public async Task<ResultModel<OperatingRoomOutputModel>> UpdateOperatingRoomOperationTypesAsync(OperatingRoomInputModel item)
        {
            var result = new ResultModel<OperatingRoomOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentoperationTypes = await _operatingRoomOperationTypeStoreService.GetByOperatingRoomIdAsync(item.Id);
            var operationTypeIds = currentoperationTypes.Select(x => x.OperationTypeId);
            var addedOperationTypes = item.OperationTypes.Except(operationTypeIds);
            var removedOperationTypes = operationTypeIds.Except(item.OperationTypes);

            foreach (var operationTypeId in addedOperationTypes)
            {
                await _operatingRoomOperationTypeStoreService.InsertAsync(new OperatingRoomOperationTypeModel
                {
                    OperationTypeId = operationTypeId,
                    OperatingRoomId = item.Id
                });
            }

            foreach (var operationType in removedOperationTypes)
            {
                await _operatingRoomOperationTypeStoreService.DeleteByIdAsync(currentoperationTypes.First(x => x.OperatingRoomId == item.Id && x.OperationTypeId == operationType).Id);
            }

            await _operatingRoomOperationTypeStoreService.SaveChangesAsync();

            return result;
        }

        public async Task<List<OperatingRoomOutputModel>> GetByOperationTypeIdAsync(int operationTypeId)
        {
            return await GetQueryable().Where(x => x.OperatingRoomOperationTypes.Any(t => t.IsActive && t.OperationTypeId == operationTypeId)).ProjectTo<OperatingRoomOutputModel>().ToListAsync();
        }

        public async Task<List<RoomInputModel>> GetOperatingRoomsForTimelineModelAsync(bool activeOnly = true)
        {
            var tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            var result =  await _context.OperatingRooms.Where(x => x.IsActive && x.IsAvailable && !x.OperatingRoomCalendars.Any(t => t.StartDate <= tomorrow && t.EndDate >= tomorrow && t.IsActive)).ProjectTo<RoomInputModel>().ToListAsync();

            if (!activeOnly)
            {
                var unavailableRooms = await _context.OperatingRooms.Where(x => x.IsActive && (!x.IsAvailable || x.OperatingRoomCalendars.Any(t => t.StartDate <= tomorrow && t.EndDate >= tomorrow && t.IsActive))).ProjectTo<RoomInputModel>().ToListAsync();

                foreach (var item in unavailableRooms)
                {
                    item.ClassName = "unavailable";
                }

                result.AddRange(unavailableRooms);
            }

            return result;
        }

        public async Task<List<RoomInputModel>> GetOperatingRoomsForDashboardTimelineModelAsync(DateTime selectDate, bool activeOnly = true)
        {
            var date = new DateTime(selectDate.Year, selectDate.Month, selectDate.Day, 0, 0, 0);
            var result = await _context.OperatingRooms.Where(x => x.IsActive && x.IsAvailable && !x.OperatingRoomCalendars.Any(t => t.StartDate <= date && t.EndDate >= date && t.IsActive)).ProjectTo<RoomInputModel>().ToListAsync();

            if (!activeOnly)
            {
                var unavailableRooms = await _context.OperatingRooms.Where(x => x.IsActive && (!x.IsAvailable || x.OperatingRoomCalendars.Any(t => t.StartDate <= date && t.EndDate >= date && t.IsActive))).ProjectTo<RoomInputModel>().ToListAsync();

                foreach (var item in unavailableRooms)
                {
                    item.ClassName = "unavailable";
                }

                result.AddRange(unavailableRooms);
            }

            return result;
        }

    }
}