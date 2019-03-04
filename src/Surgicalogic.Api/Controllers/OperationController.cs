using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
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
    public class OperationController : Controller
    {
        private readonly IOperationStoreService _operationStoreService;
        private readonly IOperationPersonnelStoreService _operationPersonnelStoreService;
        private readonly IOperationBlockedOperatingRoomStoreService _operationBlockedOperatingRoomStoreService;
        private readonly IPatientStoreService _patientStoreService;

        public OperationController(
            IOperationStoreService operationStoreService, 
            IOperationPersonnelStoreService operationPersonnelStoreService,
            IOperationBlockedOperatingRoomStoreService operationBlockedOperatingRoomStoreService,
            IPatientStoreService patientStoreService
            )
        {
            _operationStoreService = operationStoreService;
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
            return await _operationStoreService.GetAsync<OperationOutputModel>(input);
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

            excelService.Write(fs, "Worksheet", typeof(OperationExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

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
            var operationTimes = item.OperationTime.Split(':');

            var patient = new PatientModel()
            {
                FirstName = item.PatientFirstName,
                LastName = item.PatientLastName,
                IdentityNumber = item.PatientIdentityNumber
            };

            var patientModel = await _patientStoreService.InsertAndSaveAsync(patient);

            var operationItem = new OperationModel()
            {
                Name = item.Name,
                Description = item.Description,
                OperationTypeId = item.OperationTypeId,
                OperationTime = (operationTimes[0].ToNCInt() * 60) + operationTimes[1].ToNCInt(),
                Date = item.Date < new DateTime(2000, 01, 01) ? DateTime.Now.AddDays(1) : item.Date, //TODO: Çakma çözüm
                EventNumber = item.EventNumber,
                PatientId = patientModel.Result.Id
            };

            var itemDifferent = await _operationStoreService.IsEventNumberDifferent(operationItem.EventNumber);

            var result = new ResultModel<OperationOutputModel>();

            if (itemDifferent == false)
            {
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
            else
            {
                result.Info.Message = MessageType.EventNumberIsNotDifferent;
                return result;
            }
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
            var operationTimes = item.OperationTime.Split(':');

            var patient = new PatientModel()
            {
                FirstName = item.PatientFirstName,
                LastName = item.PatientLastName,
                IdentityNumber = item.PatientIdentityNumber
            };

            var patientModel = await _patientStoreService.InsertAndSaveAsync(patient);

            var operationItem = new OperationModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                OperationTypeId = item.OperationTypeId,
                OperationTime = (operationTimes[0].ToNCInt() * 60) + operationTimes[1].ToNCInt(),
                Date = item.Date < new DateTime(2000, 01, 01) ? DateTime.Now.AddDays(1) : item.Date, //TODO: Çakma çözüm
                PatientId = patientModel.Result.Id,
                EventNumber = item.EventNumber
            };

            var result = await _operationStoreService.UpdateAndSaveAsync<OperationOutputModel>(operationItem);

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