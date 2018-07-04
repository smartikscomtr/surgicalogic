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
    public class OperatingRoomController : Controller
    {
        private readonly IOperatingRoomStoreService _operatingRoomStoreService;

        public OperatingRoomController(IOperatingRoomStoreService operatingRoomStoreService)
        {
            _operatingRoomStoreService = operatingRoomStoreService;
        }

        [Route("OperatingRoom/GetOperatingRooms")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomOutputModel>> GetOperatingRooms()
        {
            return await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>();
        }

        [Route("OperatingRoom/InsertOperatingRoom")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomModel>> InsertOperatingRoom([FromBody] OperatingRoomInputModel item)
        {
            var operatingRoomItem = new OperatingRoomModel()
            {
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length,
                EquipmentId = item.EquipmentId
            };

            return await _operatingRoomStoreService.InsertAndSaveAsync(operatingRoomItem);
        }

        [Route("OperatingRoom/DeleteOperatingRoom/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperatingRoom(int id)
        {
            return await _operatingRoomStoreService.DeleteByIdAsync(id);
        }

        [Route("OperatingRoom/UpdateOperatingRoom")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomModel>> UpdateOperatingRoom([FromBody] OperatingRoomInputModel item)
        {
            var operatingRoomItem = new OperatingRoomModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length,
                EquipmentId = item.EquipmentId
            };

            return await _operatingRoomStoreService.UpdatandSaveAsync(operatingRoomItem);
        }

    }
}