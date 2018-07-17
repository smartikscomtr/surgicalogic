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

        public OperatingRoomController(IOperatingRoomStoreService operatingRoomStoreService)
        {
            _operatingRoomStoreService = operatingRoomStoreService;
        }

        /// <summary>
        /// Get operatingRoom methode
        /// </summary>
        /// <returns>OperatingRoomOutputModel list</returns>
        [Route("OperatingRoom/GetOperatingRooms")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomOutputModel>> GetOperatingRooms([FromBody]GridInputModel input)
        {
            return await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>(input);
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

            return await _operatingRoomStoreService.InsertAndSaveAsync< OperatingRoomOutputModel>(operatingRoomItem);
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
            return await _operatingRoomStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update operatingRoom methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperatingRoomModel</returns>
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
                Length = item.Length
            };

            return await _operatingRoomStoreService.UpdatandSaveAsync(operatingRoomItem);
        }
    }
}