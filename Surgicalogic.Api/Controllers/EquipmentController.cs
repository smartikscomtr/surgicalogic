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
        private readonly IOperatingRoomEquipmentStoreService _operatingRoomEquipmentStoreService;

        public EquipmentController(IEquipmentStoreService equipmentStoreService, IOperatingRoomEquipmentStoreService operatingRoomEquipmentStoreService)
        {
            _equipmentStoreService = equipmentStoreService;
            _operatingRoomEquipmentStoreService = operatingRoomEquipmentStoreService;
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
                Code = item.Code,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId
            };

            var model = await _equipmentStoreService.InsertAndSaveAsync<EquipmentOutputModel>(equipmentItem);

            if (!item.IsPortable && item.OperatingRoomIds != null && model.Info.Succeeded)
            {
                item.Id = model.Result.Id;
                await _operatingRoomEquipmentStoreService.UpdateEquipmentOperatingRoomsAsync(item);
            }

            return model;
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
            var result = await _equipmentStoreService.DeleteAndSaveByIdAsync(id);

            if (result.Result > 0)
            {
                await _operatingRoomEquipmentStoreService.DeleteByOperatingRoomIdAsync(id);
            }

            return result;
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
            var result = new ResultModel<EquipmentOutputModel>();

            var equipmentItem = new EquipmentModel()
            {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId
            };

            result = await _equipmentStoreService.UpdateAndSaveAsync<EquipmentOutputModel>(equipmentItem);

            if (!item.IsPortable && item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                await _operatingRoomEquipmentStoreService.UpdateEquipmentOperatingRoomsAsync(item);
            }

            return result;
        }
    }
}