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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperationTypeEquipmentStoreService : StoreService<OperationTypeEquipment, OperationTypeEquipmentModel>, IOperationTypeEquipmentStoreService
    {
        private DataContext _context;
        public OperationTypeEquipmentStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel<OperationTypeOutputModel>> UpdateOperationTypeEquipmentsAsync(OperationTypeInputModel item)
        {
            var result = new ResultModel<OperationTypeOutputModel>
            {
                Info = new Info
                {
                    Succeeded = true
                }
            };

            var currentEquipments = await GetByOperationTypeIdAsync(item.Id);
            var equipmentIds = currentEquipments.Select(x => x.EquipmentId);
            var addedEquipments = item.Equipments.Except(equipmentIds);
            var removedEquipments = equipmentIds.Except(item.Equipments);

            foreach (var equipmentId in addedEquipments)
            {
                await InsertAsync(new OperationTypeEquipmentModel
                {
                    EquipmentId = equipmentId,
                    OperationTypeId = item.Id
                });
            }

            foreach (var equipment in removedEquipments)
            {
                await DeleteByIdAsync(currentEquipments.First(x => x.OperationTypeId == item.Id && x.EquipmentId == equipment).Id);
            }

            await SaveChangesAsync();

            return result;
        }

        private async Task<List<OperationTypeEquipmentModel>> GetByOperationTypeIdAsync(int operationTypeId)
        {
           return await GetQueryable().Where(x => x.OperationTypeId == operationTypeId).ProjectTo<OperationTypeEquipmentModel>().ToListAsync();
        }
    }
}