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
    public class OperatingRoomCalendarController : Controller
    {
        private readonly IOperatingRoomCalendarStoreService _operatingRoomCalendarStoreService;
        public OperatingRoomCalendarController(IOperatingRoomCalendarStoreService operatingRoomCalendarStoreService)
        {
            _operatingRoomCalendarStoreService = operatingRoomCalendarStoreService;
        }

        [Route("OperatingRoomCalendar/Get")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> Get(OperatingRoomCalendarInputModel item)
        {
            return await _operatingRoomCalendarStoreService.GetByOperatingRoomIdAsync(item.OperatingRoomId);
        }

        [Route("OperatingRoomCalendar/Insert")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> Insert([FromBody] OperatingRoomCalendarInputModel item)
        {
            var model = new OperatingRoomCalendarModel
            {
                OperatingRoomId = item.OperatingRoomId,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };

            return await _operatingRoomCalendarStoreService.InsertAndSaveAsync<OperatingRoomCalendarOutputModel>(model);
        }

        [Route("OperatingRoomCalendar/Update")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> Update([FromBody] OperatingRoomCalendarInputModel item)
        {
            var model = new OperatingRoomCalendarModel
            {
                Id=item.Id,
                OperatingRoomId = item.OperatingRoomId,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };

            return await _operatingRoomCalendarStoreService.UpdateAndSaveAsync<OperatingRoomCalendarOutputModel>(model);
        }

        [Route("OperatingRoomCalendar/Delete/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperatingRoom(int id)
        {
            return await _operatingRoomCalendarStoreService.DeleteAndSaveByIdAsync(id);
        }
    }
}