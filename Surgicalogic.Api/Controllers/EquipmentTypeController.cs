using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.Threading.Tasks;


namespace Surgicalogic.Api.Controllers
{
    public class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeStoreService _equipmentTypeStoreService;

        public EquipmentTypeController(IEquipmentTypeStoreService equipmentTypeStoreService)
        {
            _equipmentTypeStoreService = equipmentTypeStoreService;
        }

        [Route("EquipmentType/GetEquipmentTypes")]
        [HttpGet]
        public async Task<ResultModel<EquipmentTypeOutputModel>> GetEquipmentTypes()
        {
            return await _equipmentTypeStoreService.GetAsync<EquipmentTypeOutputModel>();
        }

        [Route("EquipmentType/InsertEquipmentType")]
        [HttpPost]
        public async Task<ResultModel<EquipmentTypeModel>> InsertEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _equipmentTypeStoreService.InsertAndSaveAsync(equipmentTypeItem);
        }

        [Route("EquipmentType/DeleteEquipmentType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteEquipmentType(int id)
        {
            return await _equipmentTypeStoreService.DeleteByIdAsync(id);
        }

        [Route("EquipmentType/UpdateEquipmentType")]
        [HttpPost]
        public async Task<ResultModel<EquipmentTypeModel>> UpdateEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };
            return await _equipmentTypeStoreService.UpdatandSaveAsync(equipmentTypeItem);
        }


    }
}
