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
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class OperationPlanController : Controller
    {

        #region Ctor
        private readonly IOperationStoreService _operationService;
        private readonly IOperatingRoomStoreService _operatingRoomService;
        private readonly IOperationPlanStoreService _operationPlanStoreService;

        public OperationPlanController(
            IOperationStoreService operationService,
            IOperatingRoomStoreService operatingRoomService,
            IOperationPlanStoreService operationPlanStoreService
            )
        {
            _operationService = operationService;
            _operatingRoomService = operatingRoomService;
            _operationPlanStoreService = operationPlanStoreService;
        }
        #endregion

        [HttpPost]
        [Route("OperationPlan/GenerateOperationPlan")]
        public async Task<DailyPlanOutputModel> GenerateOperationPlan()
        {
            var result = new DailyPlanOutputModel();

            var operations = await _operationService.GetTomorrowOperationsAsync();
            var rooms = await _operatingRoomService.GetAvailableRoomsAsync();

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

            string req = JsonConvert.SerializeObject(surgeryPlan);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppSettings.ApiBaseUrl);

                HttpResponseMessage response = client.PostAsync(AppSettings.ApiPostUrl, new StringContent(req, Encoding.Default, "application/json")).Result;

                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResultModel = JsonConvert.DeserializeObject<DailyPlanOutputModel>(responseContent);
                    return apiResultModel;
                }
            }

            return result;
        }
    }
}