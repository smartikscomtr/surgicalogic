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

        [Route("OperatingRoomCalendar/GetOperatingRoomCalendar")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> GetOperatingRoomCalendar(OperatingRoomCalendarInputModel item)
        {
            return await _operatingRoomCalendarStoreService.GetByOperatingRoomIdAsync(item.OperatingRoomId);
        }

        [Route("OperatingRoomCalendar/InsertOperatingRoomCalendar")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> InsertOperatingRoomCalendar([FromBody] OperatingRoomCalendarInputModel item)
        {
            var model = new OperatingRoomCalendarModel
            {
                OperatingRoomId = item.OperatingRoomId,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };

            return await _operatingRoomCalendarStoreService.InsertAndSaveAsync<OperatingRoomCalendarOutputModel>(model);
        }

        [Route("OperatingRoomCalendar/UpdateOperatingRoomCalendar")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomCalendarOutputModel>> UpdateOperatingRoomCalendar([FromBody] OperatingRoomCalendarInputModel item)
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

        [Route("OperatingRoomCalendar/DeleteOperatingRoomCalendar/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperatingRoomCalendar(int id)
        {
            return await _operatingRoomCalendarStoreService.DeleteAndSaveByIdAsync(id);
        }
    }
}