using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Smartiks.Framework.IO;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
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
            var plans = await _operationPlanStoreService.GetTomorrowOperationsAsync();
            var rooms = await _operatingRoomStoreService.GetAsync<OperatingRoomForTimelineModel>();
            var tomorrow = DateTime.Now.AddDays(1) ;
            var twoDaysLater= DateTime.Now.AddDays(2);

            var roomTimelineModel = AutoMapper.Mapper.Map<List<OperatingRoomForTimelineModel>>(rooms.Result);

            var result = new ResultModel<OperationPlanOutputModel>
            {
                Info = new Info(),
                Result = new OperationTimelineOutputModel(plans, rooms.Result)
                {
                    MinDate = tomorrow.ToString("yyyy-MM-dd 00:00:00"),
                    MaxDate = twoDaysLater.ToString("yyyy-MM-dd 00:00:00"),
                    StartDate = plans.Select(x => x.start).Min(),
                    EndDate = plans.Select(x => x.end).Max(),
                    Period = AppSettings.PeriodInMinutes
                }
            };

            return result;
        }

        [Route("OperationPlan/GetOperationPlanHistory")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetOperationPlanHistory(GridInputModel input)
        {
            return await _operationPlanStoreService.GetAsync<OperationPlanHistoryOutputModel>(input);
        }

        [Route("OperationPlan/GetTomorrowOperationPlans")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationPlans(GridInputModel input)
        {
            return await _operationPlanStoreService.GetAsync<OperationPlanHistoryOutputModel>(input);
        }

        [Route("OperationPlan/GetTomorrowOperationList")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationList(GridInputModel input)
        {
            return await _operationPlanStoreService.GetTomorrowOperationListAsync(input);
        }
       

        [HttpPost]
        [Route("OperationPlan/GenerateOperationPlan")]
        public async Task<DailyPlanOutputModel> GenerateOperationPlan()
        {
            var result = new DailyPlanOutputModel();

            var tomorrowOperations = await _operationStoreService.GetTomorrowOperationsAsync();
            var rooms = await _operatingRoomStoreService.GetAvailableRoomsAsync();

            var operations = new List<Planning.Model.InputModel.OperationInputModel>();

            foreach (var operation in tomorrowOperations)
            {
                //Bu operasyonun yapılabileceği odaları, operasyonun tipi üzerinden giderek buluyorum.
                var operatingRoomIds = operation.OperationType.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperatingRoomId);

                var outputModel = AutoMapper.Mapper.Map<Model.OutputModel.OperationOutputModel>(operation);

                operations.Add(new Planning.Model.InputModel.OperationInputModel
                {
                    Id = operation.Id,
                    Name = operation.Name,
                    Period = outputModel.Period,
                    DoctorIds = outputModel.DoctorIds.ToArray(),
                    //Bu operasyonun yapılamayacağı odaları, tüm odalardan yapılabileceği odaları çıkartarak buluyorum.
                    UnavailableRooms = rooms.Select(x => x.Id).Except(operatingRoomIds.Except(outputModel.BlockedOperatingRoomIds)).ToList()
                });
            }

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

        [Route("OperationPlan/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("OperationHistory_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operationPlanStoreService.GetExportAsync<OperationPlanExportModel>();

            excelService.Write(fs, "Worksheet", typeof(OperationPlanExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
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