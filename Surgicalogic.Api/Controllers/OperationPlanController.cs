using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class OperationPlanController : Controller
    {

        #region Ctor
        private readonly IOperationStoreService _operationStoreService;
        private readonly IOperatingRoomStoreService _operatingRoomStoreService;
        private readonly IOperationPlanStoreService _operationPlanStoreService;

        public OperationPlanController(
            IOperationStoreService operationStoreService,
            IOperatingRoomStoreService operatingRoomStoreService,
            IOperationPlanStoreService operationPlanStoreService
            )
        {
            _operationStoreService = operationStoreService;
            _operatingRoomStoreService = operatingRoomStoreService;
            _operationPlanStoreService = operationPlanStoreService;
        }
        #endregion

        [Route("OperationPlan/GetOperationPlans")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanOutputModel>> GetOperationPlans(GridInputModel input)
        {
            var plans = await _operationPlanStoreService.GetAsync<OperationPlanOutputModel>(input);
            var rooms = await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>();
            var tomorrow = DateTime.Now.AddDays(1) ;
            var twoDaysLater= DateTime.Now.AddDays(2);

            var roomTimelineModel = AutoMapper.Mapper.Map<List<OperatingRoomForTimelineModel>>(rooms.Result);

            var result = new OperationTimelineOutputModel(plans.Result, roomTimelineModel);

            return new ResultModel<OperationPlanOutputModel>
            {
                Info = new Info(),
                Result = new OperationTimelineOutputModel(plans.Result, rooms.Result)
                {
                    MinDate = tomorrow.ToString("yyyy-MM-dd 00:00:00"),
                    MaxDate = twoDaysLater.ToString("yyyy-MM-dd 00:00:00"),
                    EndDate = tomorrow.ToString("yyyy-MM-dd 14:00:00"),
                    Period = AppSettings.PeriodInMinutes
                        
                }
            };


            return result;
        }

        [HttpPost]
        [Route("OperationPlan/GenerateOperationPlan")]
        public async Task<DailyPlanOutputModel> GenerateOperationPlan()
        {
            var result = new DailyPlanOutputModel();

            var operations = await _operationStoreService.GetTomorrowOperationsAsync();
            var rooms = await _operatingRoomStoreService.GetAvailableRoomsAsync();

            var surgeryPlan = new DailyPlanInputModel
            {
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 32,
                    MaximumPeriod = 64,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = AppSettings.PeriodInMinutes,
                },
                Rooms = rooms,
                Operations = operations
            };

            foreach (var item in operations)
            {
                //item.UnavailableRooms = item
            }

            string req = JsonConvert.SerializeObject(surgeryPlan);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppSettings.ApiBaseUrl);
                HttpResponseMessage response = await client.PostAsync(AppSettings.ApiPostUrl, new StringContent(req, Encoding.Default, "application/json"));

                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResultModel = JsonConvert.DeserializeObject<DailyPlanOutputModel>(responseContent);

                    foreach (var room in apiResultModel.Rooms)
                    {
                        foreach (var operation in room.Operations)
                        {
                            var model = new OperationPlanModel
                            {
                                OperatingRoomId = room.Id,
                                OperationId = operation.Id,
                                OperationDate = operation.StartDate
                            };

                            await _operationPlanStoreService.InsertAsync(model);
                        }
                    }

                    await _operationPlanStoreService.SaveChangesAsync();
                    return apiResultModel;
                }
            }

            return result;
        }

        [HttpPost]
        [Route("OperationPlan/UpdateOperationPlan")]
        public async Task<ResultModel<OperationPlanOutputModel>> UpdateOperationPlan([FromBody] OperationPlanInputModel item)
        {
            var operationPlan = new OperationPlanModel
            {
                Id = item.Id,
                OperatingRoomId = item.OperatingRoomId,
                OperationId = item.OperationId,
                OperationDate = item.OperationDate
            };

            return await _operationPlanStoreService.UpdateAndSaveAsync<OperationPlanOutputModel>(operationPlan);
        }
    }
}