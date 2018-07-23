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
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class OperatingRoomController : Controller
    {
        private readonly IOperatingRoomStoreService _operatingRoomStoreService;
        private readonly IOperatingRoomEquipmentStoreService _operatingRoomEquipmentStoreService;

        public OperatingRoomController(IOperatingRoomStoreService operatingRoomStoreService, IOperatingRoomEquipmentStoreService operatingRoomEquipmentStoreService)
        {
            _operatingRoomStoreService = operatingRoomStoreService;
            _operatingRoomEquipmentStoreService = operatingRoomEquipmentStoreService;
        }

        /// <summary>
        /// Get operatingRoom methode
        /// </summary>
        /// <returns>OperatingRoomOutputModel list</returns>
        [Route("OperatingRoom/GetOperatingRooms")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomOutputModel>> GetOperatingRooms(GridInputModel input)
        {
            var result = await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>(input);
            return result;
        }

        [Route("OperatingRoom/GetAllOperatingRooms")]
        public async Task<ResultModel<OperatingRoomOutputModel>> GetAllOperatingRooms()
        {
            return await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>();
        }

        /// <summary>
        /// Add operatingRoom methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperatingRoomOutputModel</returns>
        [Route("OperatingRoom/InsertOperatingRoom")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomOutputModel>> InsertOperatingRoom([FromBody] OperatingRoomInputModel item)
        {
            var operatingRoomItem = new OperatingRoomModel()
            {
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length
            };

            if (item.Equipments != null && item.Equipments.Count > 0)
            {
                bool isEquipmentsRelatedToOperatingRoom = await _operatingRoomEquipmentStoreService.CheckEquipmentsRelatedToOperationRoom(item.Equipments.ToArray());

                if (isEquipmentsRelatedToOperatingRoom)
                {
                    return new ResultModel<OperatingRoomOutputModel> { Info = new Info { Succeeded = false, InfoType = Model.Enum.InfoType.Error, Message = Model.Enum.MessageType.EquipmentRelatedToOperatingRoom } };
                }
            }            

            var model = await _operatingRoomStoreService.InsertAndSaveAsync<OperatingRoomOutputModel>(operatingRoomItem);

            item.Id = model.Result.Id;

            if (model.Info.Succeeded && item.Equipments != null && item.Equipments.Count > 0)
            {
                var result = await _operatingRoomStoreService.UpdateOperatingRoomEquipmentsAsync(item);

                if (!result.Info.Succeeded)
                {
                    return result;
                }
            }

            if (model.Info.Succeeded && item.OperationTypes != null && item.OperationTypes.Count > 0)
            {
                var result = await _operatingRoomStoreService.UpdateOperatingRoomOperationTypesAsync(item);

                if (!result.Info.Succeeded)
                {
                    return result;
                }
            }

            return model;
        }

        /// <summary>
        /// Remove operatingRoom item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("OperatingRoom/DeleteOperatingRoom/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperatingRoom(int id)
        {
            return await _operatingRoomStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update operatingRoom methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperatingRoomModel</returns>
        [Route("OperatingRoom/UpdateOperatingRoom")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomOutputModel>> UpdateOperatingRoom([FromBody] OperatingRoomInputModel item)
        {
            var model = new OperatingRoomModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length
            };

            if (item.Equipments != null)
            {
                var result =  await _operatingRoomStoreService.UpdateOperatingRoomEquipmentsAsync(item);

                if (!result.Info.Succeeded)
                {
                    return result;
                }
            }

            if (item.OperationTypes != null)
            {
                var result = await _operatingRoomStoreService.UpdateOperatingRoomOperationTypesAsync(item);

                if (!result.Info.Succeeded)
                {
                    return result;
                }
            }

            return await _operatingRoomStoreService.UpdateAndSaveAsync<OperatingRoomOutputModel>(model);
        }
    }
}