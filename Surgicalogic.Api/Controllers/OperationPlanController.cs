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
        private readonly IOperationPlanHistoryStoreService _operationPlanHistoryStoreService;

        public OperationPlanController(
            IOperationStoreService operationStoreService,
            IOperatingRoomStoreService operatingRoomStoreService,
            IOperationPlanStoreService operationPlanStoreService,
            IOperationPlanHistoryStoreService operationPlanHistoryStoreService
            )
        {
            _operationStoreService = operationStoreService;
            _operatingRoomStoreService = operatingRoomStoreService;
            _operationPlanStoreService = operationPlanStoreService;
            _operationPlanHistoryStoreService = operationPlanHistoryStoreService;
        }
        #endregion

        [Route("OperationPlan/GetOperationPlans")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanOutputModel>> GetOperationPlans(GridInputModel input)
        {
            var plans = await _operationPlanStoreService.GetTomorrowOperationsAsync();
            var rooms = await _operatingRoomStoreService.GetOperatingRoomsForTimelineModelAsync();

            var tomorrow = DateTime.Now.AddDays(1);
            var twoDaysLater = DateTime.Now.AddDays(2);

            var roomTimelineModel = AutoMapper.Mapper.Map<List<OperatingRoomForTimelineModel>>(rooms);

            var result = new ResultModel<OperationPlanOutputModel>
            {
                Info = new Info(),
                Result = new OperationTimelineOutputModel(plans, roomTimelineModel)
                {
                    MinDate = tomorrow.ToString("yyyy-MM-dd 00:00:00"),
                    MaxDate = twoDaysLater.ToString("yyyy-MM-dd 00:00:00"),
                    StartDate = plans.Select(x => x.start).Min() ?? tomorrow.ToString("yyyy-MM-dd 00:00:00"),
                    EndDate = plans.Select(x => x.end).Max() ?? twoDaysLater.ToString("yyyy-MM-dd 00:00:00"),
                    Period = AppSettings.PeriodInMinutes,
                    WorkingHourStart = AppSettings.WorkingHourStart,
                    WorkingHourEnd = AppSettings.WorkingHourEnd
                }
            };

            return result;
        }

        [Route("OperationPlan/GetOperationPlanHistory")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetOperationPlanHistory(GridInputModel input)
        {
            return await _operationPlanHistoryStoreService.GetAsync<OperationPlanHistoryOutputModel>(input);
        }

        [Route("OperationPlan/GetTomorrowOperationList")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanHistoryOutputModel>> GetTomorrowOperationList(GridInputModel input)
        {
            return await _operationPlanHistoryStoreService.GetTomorrowOperationListAsync(input);
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
                var personnelIds = operation.OperationPersonels.Where(x => x.Personnel.PersonnelTitle.SuitableForMultipleOperation != true);

                var outputModel = AutoMapper.Mapper.Map<Model.OutputModel.OperationOutputModel>(operation);

                operations.Add(new Planning.Model.InputModel.OperationInputModel
                {
                    Id = operation.Id,
                    Name = operation.Name,
                    Period = outputModel.Period,
                    DoctorIds = personnelIds.Select(x => x.PersonnelId).ToArray(),
                    //Bu operasyonun yapılamayacağı odaları, tüm odalardan yapılabileceği odaları çıkartarak buluyorum.
                    UnavailableRooms = rooms.Select(x => x.Id).Except(operatingRoomIds.Except(outputModel.BlockedOperatingRoomIds)).ToList()
                });
            }

            var surgeryPlan = new DailyPlanInputModel
            {
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = Convert.ToInt32(AppSettings.WorkingHourEnd.Subtract(AppSettings.WorkingHourStart).TotalMinutes) / AppSettings.PeriodInMinutes,
                    MaximumPeriod = Convert.ToInt32((new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day) - AppSettings.WorkingHourStart).TotalMinutes) / AppSettings.PeriodInMinutes,
                    StartingHour = AppSettings.WorkingHourStart.Hour,
                    StartingMinute = AppSettings.WorkingHourStart.Minute,
                    PeriodInMinutes = AppSettings.PeriodInMinutes,
                },
                Rooms = rooms,
                Operations = operations
            };

            string req = JsonConvert.SerializeObject(surgeryPlan);

            using (var client = new HttpClient() { BaseAddress = new Uri(AppSettings.ApiBaseUrl), Timeout = TimeSpan.FromMinutes(10) })
            {
                HttpResponseMessage response = await client.PostAsync(AppSettings.ApiPostUrl, new StringContent(req, Encoding.Default, "application/json"));

                if (response.Content != null)
                {
                    await _operationPlanStoreService.DeleteTomorrowPlanAsync();

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
                                OperationDate = operation.StartDate,
                                RealizedStartDate = operation.StartDate,
                                RealizedEndDate = operation.StartDate.AddMinutes(operation.Period * AppSettings.PeriodInMinutes)
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
        public async Task UpdateOperationPlan([FromBody] List<OperationPlanInputModel> items)
        {
            var updatedItemIds = items.Select(x => x.Id).ToArray();
            var updatedOperationIds = items.Select(x => x.OperationId).ToArray();

            var updatedPlanItems = await _operationPlanStoreService.GetByIdListAsync(updatedItemIds);
            var updatedOperationItems = await _operationStoreService.GetByIdListAsync(updatedOperationIds);

            foreach (var plan in updatedPlanItems)
            {
                var item = items.First(x => x.Id == plan.Id);
                plan.OperationDate = item.Start.AddHours(3);
                plan.RealizedStartDate = plan.OperationDate;
                plan.OperatingRoomId = item.RoomId;
                plan.RealizedEndDate = plan.RealizedStartDate.AddMinutes(item.Length);
                await _operationPlanStoreService.UpdateAsync(plan);
            }

            foreach (var operation in updatedOperationItems)
            {
                var item = items.First(x => x.OperationId == operation.Id);
                operation.OperationTime = item.Length;
                await _operationStoreService.UpdateAsync(operation);
            }

            await _operationPlanStoreService.SaveChangesAsync();
        }
    }
}