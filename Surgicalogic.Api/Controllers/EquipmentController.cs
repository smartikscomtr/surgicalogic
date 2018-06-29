using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System;
using System.Threading.Tasks;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentStoreService _equipmentStoreService;

        public EquipmentController(IEquipmentStoreService equipmentStoreService)
        {
            _equipmentStoreService = equipmentStoreService;
        }

        // GET api/values
        [Route("Equipment/GetEquipments")]
        [HttpGet]
        public async Task<ResultModel<EquipmentOutputModel>> GetEquipments([FromQuery] StringFilterSortPaginationModel<EquipmentSorting, EquipmentFilter> filter = null)
        {
            return await _equipmentStoreService.GetAsync<EquipmentOutputModel>(filter);
        }

        [Route("Equipment/InsertEquipment")]
        [HttpPost]
        public async Task<ResultModel<EquipmentModel>> InsertEquipment([FromBody] EquipmentInputModel item)
        {
            var equipmentItem = new EquipmentModel()
            {
                Name = item.Name,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId,                                
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return await _equipmentStoreService.InsertAsync(equipmentItem);
        }

        [Route("Equipment/DeleteEquipment/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteEquipmentType(int id)
        {
            var result = await _equipmentStoreService.DeleteByIdAsync(id);
            return Json(result);
        }

        [Route("Equipment/UpdateEquipment")]
        [HttpPost]
        public Task<ResultModel<EquipmentModel>> UpdateEquipments([FromBody] EquipmentInputModel item)
        {
            var equipmentItem = new EquipmentModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId,
                ModifiedDate = DateTime.Now,
                ModifiedBy = 2
            };
            return _equipmentStoreService.UpdateAsync(equipmentItem);
        }
       
    }
}