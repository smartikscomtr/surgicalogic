using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Smartiks.Framework.IO;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class OperationPlanController : Controller
    {

        #region Ctor
        private readonly IOperationStoreService _operationStoreService;
        private readonly IOperatingRoomStoreService _operatingRoomStoreService;
        private readonly IOperationPlanStoreService _operationPlanStoreService;
        private readonly ISettingStoreService _settingStoreService;

        public OperationPlanController(
            IOperationStoreService operationStoreService,
            IOperatingRoomStoreService operatingRoomStoreService,
            IOperationPlanStoreService operationPlanStoreService,
            ISettingStoreService settingStoreService
            )
        {
            _operationStoreService = operationStoreService;
            _operatingRoomStoreService = operatingRoomStoreService;
            _operationPlanStoreService = operationPlanStoreService;
            _settingStoreService = settingStoreService;
        }
        #endregion

        [Route("OperationPlan/GetDashboardTimelinePlans/{selectDate:DateTime}")]
        [HttpGet]
        public async Task<ResultModel<OperationPlanOutputModel>> GetDashboardTimelinePlans(DateTime selectDate)
        {
            var plans = await _operationPlanStoreService.GetDashboardTimelineOperationsAsync(selectDate);
            var rooms = await _operatingRoomStoreService.GetOperatingRoomsForDashboardTimelineModelAsync(selectDate, true);

            foreach (var item in plans)
            {
                if (item.RealizedStartDate > DateTime.Now)
                {
                    item.className = "blue";
                }
                else if (item.RealizedStartDate < DateTime.Now && item.RealizedEndDate > DateTime.Now)
                {
                    item.className = "red";
                }
                else if (item.RealizedEndDate < DateTime.Now)
                {
                    item.className = "green";
                }
            }

            var selectDay = selectDate;
            var selectDaysLater = selectDate.AddDays(1);

            var roomTimelineModel = AutoMapper.Mapper.Map<List<OperatingRoomForTimelineModel>>(rooms);

            var systemSettings = await _settingStoreService.GetAllAsync();

            var workingHourStart = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationWorkingHourStart.ToString());
            var workingHourEnd = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationWorkingHourEnd.ToString());
            var period = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationPeriodInMinutes.ToString());

            var result = new ResultModel<OperationPlanOutputModel>
            {
                Info = new Info(),
                Result = new OperationTimelineOutputModel(plans, roomTimelineModel)
                {
                    MinDate = selectDay.ToString("yyyy-MM-dd 00:00:00"),
                    MaxDate = selectDaysLater.ToString("yyyy-MM-dd 00:00:00"),
                    StartDate = plans.Select(x => x.start).Min() ?? selectDay.ToString("yyyy-MM-dd 00:00:00"),
                    EndDate = plans.Select(x => x.end).Max() ?? selectDaysLater.ToString("yyyy-MM-dd 00:00:00"),
                    Period = period.IntValue.Value,
                    WorkingHourStart = workingHourStart.TimeValue.HourToDateTime(),
                    WorkingHourEnd = workingHourEnd.TimeValue.HourToDateTime(),
                }
            };

            return result;
        }

        [HttpPost]
        [Route("OperationPlan/GenerateOperationPlan")]
        public async Task<DailyPlanOutputModel> GenerateOperationPlan([FromBody]GenerateOperationPlanInputModel input)
        {
            var result = new DailyPlanOutputModel();

            var allOperations = await _operationStoreService.GetOperationsByDateAsync(input.OperationDate);
            var rooms = await _operatingRoomStoreService.GetAvailableRoomsAsync(input.OperationDate);

            var operations = new List<Planning.Model.InputModel.OperationInputModel>();

            var systemSettings = await _settingStoreService.GetAllAsync();

            var workingHourStart = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationWorkingHourStart.ToString());
            var workingHourEnd = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationWorkingHourEnd.ToString());
            var period = systemSettings.SingleOrDefault(x => x.Key == SettingKey.OperationPeriodInMinutes.ToString());

            foreach (var operation in allOperations)
            {
                //Bu operasyonun yapılabileceği odaları, operasyonun tipi üzerinden giderek buluyorum.
                var operatingRoomIds = operation.OperationType.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperatingRoomId);
                var personnelIds = operation.OperationPersonels.Where(x => x.Personnel.PersonnelCategory.SuitableForMultipleOperation != true);

                var outputModel = AutoMapper.Mapper.Map<Model.OutputModel.OperationOutputModel>(operation);

                operations.Add(new Planning.Model.InputModel.OperationInputModel
                {
                    Id = operation.Id,
                    Name = operation.Name,
                    Period = operation.OperationTime % period.IntValue.Value == 0 ? operation.OperationTime / period.IntValue.Value : operation.OperationTime / period.IntValue.Value + 1,
                    DoctorIds = personnelIds.Select(x => x.PersonnelId).ToArray(),
                    OperationTime = operation.OperationTime,
                    //Bu operasyonun yapılamayacağı odaları, tüm odalardan yapılabileceği odaları çıkartarak buluyorum.
                    UnavailableRooms = rooms.Select(x => x.Id).Except(operatingRoomIds.Except(outputModel.BlockedOperatingRoomIds)).ToList()
                });
            }

            var surgeryPlan = new DailyPlanInputModel
            {
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = Convert.ToInt32(workingHourEnd.TimeValue.HourToDateTime().Subtract(workingHourStart.TimeValue.HourToDateTime()).TotalMinutes) / period.IntValue.Value,
                    MaximumPeriod = Convert.ToInt32((new DateTime(input.OperationDate.AddDays(1).Year, input.OperationDate.AddDays(1).Month, input.OperationDate.AddDays(1).Day) - workingHourStart.TimeValue.HourToDateTime()).TotalMinutes) / period.IntValue.Value,
                    StartingHour = workingHourStart.TimeValue.HourToDateTime().Hour,
                    StartingMinute = workingHourStart.TimeValue.HourToDateTime().Minute,
                    PeriodInMinutes = period.IntValue.Value,
                    OperationDate = input.OperationDate
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
                    await _operationPlanStoreService.DeletePlanByDateAsync(input.OperationDate);

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
                                RealizedEndDate = operation.StartDate.AddMinutes(operation.OperationTime)
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