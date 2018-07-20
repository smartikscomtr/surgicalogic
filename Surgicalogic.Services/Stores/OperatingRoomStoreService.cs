using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Services.Stores.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomStoreService : StoreService<OperatingRoom, OperatingRoomModel>, IOperatingRoomStoreService
    {
        private DataContext _context;
        private IOperatingRoomEquipmentStoreService _operatingRoomEquipmentStoreService;

        public OperatingRoomStoreService(
            DataContext context,
            IOperatingRoomEquipmentStoreService operatingRoomEquipmentStoreService
            ) : base(context)
        {
            _context = context;
            _operatingRoomEquipmentStoreService = operatingRoomEquipmentStoreService;
        }

        public async Task<ResultModel<OperatingRoomModel>> UpdateOperatingRoomEquipmentsAsync(OperatingRoomInputModel item)
        {
            var result = new ResultModel<OperatingRoomModel>
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

            bool isAnyEquipmentRelatedOperatingRoom = await _operatingRoomEquipmentStoreService.CheckEquipmentsRelatedOperationRoom(addedEquipments.ToArray());

            if (isAnyEquipmentRelatedOperatingRoom)
            {
                result.Info = new Info
                {
                    Succeeded = false,
                    InfoType = Model.Enum.InfoType.Error,
                    Message = Model.Enum.MessageType.EquipmentRelatedToOperatingRoom
                };

                return result;
            }

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
    }
}