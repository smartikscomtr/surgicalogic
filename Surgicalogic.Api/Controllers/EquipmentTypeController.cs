using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
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
        public async Task<ResultModel<EquipmentTypeModel>> GetEquipmentTypes([FromQuery] StringFilterSortPaginationModel<EquipmentTypeSorting, EquipmentTypeFilter> filter = null)
        {
            return await _equipmentTypeStoreService.GetAsync(filter);
        }

        [Route("EquipmentType/InsertEquipmentType")]
        [HttpPost]
        public async Task<int> InsertEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return await _equipmentTypeStoreService.InsertAsync(equipmentTypeItem);
        }

        [Route("EquipmentType/DeleteEquipmentType/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteEquipmentType(int id)
        {
            var result = await _equipmentTypeStoreService.DeleteByIdAsync(id);
            return Json(result);
        }

        [Route("EquipmentType/UpdateEquipmentType")]
        [HttpPost]
        public Task UpdateEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };
            return _equipmentTypeStoreService.UpdateAsync(equipmentTypeItem);
        }


    }
}
