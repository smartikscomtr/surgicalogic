using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentStoreService _equipmentStoreService;

        public EquipmentController(IEquipmentStoreService equipmentStoreService)
        {
            _equipmentStoreService = equipmentStoreService;
        }

        /// <summary>
        /// Get equipment methode
        /// </summary>
        /// <returns>EquipmentOutputModel list</returns>
        [Route("Equipment/GetEquipments")]
        [HttpGet]
        public async Task<ResultModel<EquipmentOutputModel>> GetEquipments(GridInputModel item)
        {
            var result = await _equipmentStoreService.GetAsync<EquipmentOutputModel>(item);
            return result;
        }

        [Route("Equipment/GetAllEquipments")]
        public async Task<ResultModel<EquipmentOutputModel>> GetAllEquipments()
        {
            return await _equipmentStoreService.GetAsync<EquipmentOutputModel>();
        }

        [Route("Equipment/GetNonPortableEquipments")]
        public async Task<ResultModel<EquipmentOutputModel>> GetNonPortableEquipments()
        {
            return await _equipmentStoreService.GetNonPortableEquipments();
        }

        /// <summary>
        /// Add equipment methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentOutputModel</returns>
        [Route("Equipment/InsertEquipment")]
        [HttpPost]
        public async Task<ResultModel<EquipmentOutputModel>> InsertEquipment([FromBody] EquipmentInputModel item)
        {
            var equipmentItem = new EquipmentModel()
            {
                Name = item.Name,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId
            };

            return await _equipmentStoreService.InsertAndSaveAsync<EquipmentOutputModel>(equipmentItem);
        }

        /// <summary>
        /// Remove equipment item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Equipment/DeleteEquipment/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteEquipmentType(int id)
        {
            return await _equipmentStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update equipment methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentModel</returns>
        [Route("Equipment/UpdateEquipment")]
        [HttpPost]
        public async Task<ResultModel<EquipmentOutputModel>> UpdateEquipments([FromBody] EquipmentInputModel item)
        {
            var equipmentItem = new EquipmentModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId
            };

            return await _equipmentStoreService.UpdateAndSaveAsync<EquipmentOutputModel>(equipmentItem);
        }
    }
}