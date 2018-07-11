using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeStoreService _equipmentTypeStoreService;

        public EquipmentTypeController(IEquipmentTypeStoreService equipmentTypeStoreService)
        {
            _equipmentTypeStoreService = equipmentTypeStoreService;
        }

        /// <summary>
        /// Get equipmentType methode
        /// </summary>
        /// <returns>EquipmentTypeOutputModel list</returns>
        [Route("EquipmentType/GetEquipmentTypes")]
        [HttpGet]
        public async Task<ResultModel<EquipmentTypeOutputModel>> GetEquipmentTypes()
        {
            return await _equipmentTypeStoreService.GetAsync<EquipmentTypeOutputModel>();
        }

        /// <summary>
        /// Add equipmentType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentTypeOutputModel</returns>
        [Route("EquipmentType/InsertEquipmentType")]
        [HttpPost]
        public async Task<ResultModel<EquipmentTypeOutputModel>> InsertEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _equipmentTypeStoreService.InsertAndSaveAsync<EquipmentTypeOutputModel>(equipmentTypeItem);
        }

        /// <summary>
        /// Remove equipmentType item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("EquipmentType/DeleteEquipmentType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteEquipmentType(int id)
        {
            return await _equipmentTypeStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update equipmentType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentTypeModel</returns>
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