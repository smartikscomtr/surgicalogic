using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    [Authorize]
    public class OperationController : Controller
    {
        private readonly IOperationStoreService _operationStoreService;
        private readonly IOperationGridStoreService _operationGridStoreService;
        private readonly IOperationPersonnelStoreService _operationPersonnelStoreService;
        private readonly IOperationBlockedOperatingRoomStoreService _operationBlockedOperatingRoomStoreService;
        private readonly IPatientStoreService _patientStoreService;

        public OperationController(
            IOperationStoreService operationStoreService,
            IOperationGridStoreService operationGridStoreService,
            IOperationPersonnelStoreService operationPersonnelStoreService,
            IOperationBlockedOperatingRoomStoreService operationBlockedOperatingRoomStoreService,
            IPatientStoreService patientStoreService
            )
        {
            _operationStoreService = operationStoreService;
            _operationGridStoreService = operationGridStoreService;
            _operationPersonnelStoreService = operationPersonnelStoreService;
            _operationBlockedOperatingRoomStoreService = operationBlockedOperatingRoomStoreService;
            _patientStoreService = patientStoreService;
        }

        /// <summary>
        /// Get Operation methode
        /// </summary>
        /// <returns>OperationOutputModel list</returns>
        [Route("Operation/GetOperations")]
        [HttpGet]
        public async Task<ResultModel<OperationOutputModel>> GetOperations(GridInputModel input)
        {
            return await _operationGridStoreService.GetAsync<OperationOutputModel>(input);
        }

        [Route("Operation/GetOperationNamesForHistory")]
        public async Task<List<OperationNameModel>> GetOperationNamesForHistory()
        {
            return await _operationStoreService.GetOperationNamesForHistory();
        }

        [Route("Operation/GetAllOperations")]
        public async Task<ResultModel<OperationOutputModel>> GetAllOperations()
        {
            return await _operationStoreService.GetAsync<OperationOutputModel>();
        }


        [Route("Operation/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Operations_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operationStoreService.GetExportAsync<OperationExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(OperationExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add Operation methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationOutputModel</returns>
        [Route("Operation/InsertOperation")]
        [HttpPost]
        public async Task<ResultModel<OperationOutputModel>> InsertOperation([FromBody] OperationInputModel item)
        {
            var result = new ResultModel<OperationOutputModel>();

            var isDuplicateEventNumber = await _operationStoreService.IsDuplicateEventNumber(item.EventNumber, item.Id);

            if (isDuplicateEventNumber)
            {
                result.Info = new Info
                {
                    Succeeded = false,
                    InfoType = Model.Enum.InfoType.Error,
                    Message = Model.Enum.MessageType.EventNumberIsNotDifferent
                };

                return result;
            }

            var operationTimes = item.OperationTime.Split(':');

            var operationItem = new OperationModel()
            {
                Name = item.Name,
                Description = item.Description,
                OperationTypeId = item.OperationTypeId,
                OperationTime = (operationTimes[0].ToNCInt() * 60) + operationTimes[1].ToNCInt(),
                Date = item.Date < new DateTime(2000, 01, 01) ? DateTime.Now.AddDays(1) : item.Date, //TODO: Çakma çözüm
                PatientId = item.PatientId,
                EventNumber = item.EventNumber
            };

            result = await _operationStoreService.InsertAndSaveAsync<OperationOutputModel>(operationItem);

            item.Id = result.Result.Id;

            if (item.PersonnelIds != null && result.Info.Succeeded)
            {
                await _operationPersonnelStoreService.UpdateOperationPersonnelsAsync(item);
            }

            if (item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                await _operationBlockedOperatingRoomStoreService.UpdateOperatingRoomsAsync(item);
            }

            return result;
        }

        /// <summary>
        /// Remove Operation item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Operation/DeleteOperation/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperation(int id)
        {
            return await _operationStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update Operation methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationModel</returns>
        [Route("Operation/UpdateOperation")]
        [HttpPost]
        public async Task<ResultModel<OperationOutputModel>> UpdateOperation([FromBody] OperationInputModel item)
        {
            var result = new ResultModel<OperationOutputModel>();

            var isDuplicateEventNumber = await _operationStoreService.IsDuplicateEventNumber(item.EventNumber, item.Id);

            if (isDuplicateEventNumber)
            {
                result.Info = new Info
                {
                    Succeeded = false,
                    InfoType = Model.Enum.InfoType.Error,
                    Message = Model.Enum.MessageType.EventNumberIsNotDifferent
                };

                return result;
            }

            var operationTimes = item.OperationTime.Split(':');

            var operationItem = new OperationModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                OperationTypeId = item.OperationTypeId,
                OperationTime = (operationTimes[0].ToNCInt() * 60) + operationTimes[1].ToNCInt(),
                Date = item.Date < new DateTime(2000, 01, 01) ? DateTime.Now.AddDays(1) : item.Date, //TODO: Çakma çözüm
                PatientId = item.PatientId,
                EventNumber = item.EventNumber
            };

            result = await _operationStoreService.UpdateAndSaveAsync<OperationOutputModel>(operationItem);

            if (item.PersonnelIds != null && result.Info.Succeeded)
            {
                await _operationPersonnelStoreService.UpdateOperationPersonnelsAsync(item);
            }

            if (item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                await _operationBlockedOperatingRoomStoreService.UpdateOperatingRoomsAsync(item);
            }

            return result;
        }
    }
}