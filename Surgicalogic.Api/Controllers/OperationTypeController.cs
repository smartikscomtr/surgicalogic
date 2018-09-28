﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class OperationTypeController : Controller
    {
        private readonly IOperationTypeStoreService _operationTypeStoreService;
        private readonly IOperationTypeEquipmentStoreService _operationTypeEquipmentStoreService;
        private readonly IOperatingRoomOperationTypeStoreService _operatingRoomOperationTypeStoreService;
        public OperationTypeController(
            IOperationTypeStoreService operationTypeStoreService,
            IOperationTypeEquipmentStoreService operationTypeEquipmentStoreService,
            IOperatingRoomOperationTypeStoreService operatingRoomOperationTypeStoreService
            )
        {
            _operationTypeStoreService = operationTypeStoreService;
            _operationTypeEquipmentStoreService = operationTypeEquipmentStoreService;
            _operatingRoomOperationTypeStoreService = operatingRoomOperationTypeStoreService;
        }

        /// <summary>
        /// Get operationType methode
        /// </summary>
        /// <returns>OperationTypeOutputModel list</returns>
        [Route("OperationType/GetOperationTypes")]
        [HttpGet]
        public async Task<ResultModel<OperationTypeOutputModel>> GetOperationTypes(GridInputModel input)
        {
            return await _operationTypeStoreService.GetAsync<OperationTypeOutputModel>(input);
        }

        /// <summary>
        /// Get operationType methode
        /// </summary>
        /// <returns>OperationTypeOutputModel list</returns>
        [Route("OperationType/GetAllOperationTypes")]
        [HttpGet]
        public async Task<ResultModel<OperationTypeOutputModel>> GetAllOperationTypes()
        {
            return await _operationTypeStoreService.GetAsync<OperationTypeOutputModel>();
        }

        [Route("OperationType/GetOperationTypesByBranchId/{branchId:int}")]
        [HttpGet]
        public async Task<List<OperationTypeForOperationOutputModel>> GetOperationTypesByBranchId(int branchId)
        {
            return await _operationTypeStoreService.GetByBranchIdAsync(branchId);
        }

        [Route("OperationType/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("OperationTypes_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operationTypeStoreService.GetExportAsync<OperationTypeExportModel>();

            excelService.Write(fs, "Worksheet", typeof(OperationTypeExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add operationType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationTypeOutputModel</returns>
        [Route("OperationType/InsertOperationType")]
        public async Task<ResultModel<OperationTypeOutputModel>> InsertOperationType([FromBody] OperationTypeInputModel item)
        {
            var operationTypeItem = new OperationTypeModel()
            {
                Name = item.Name,
                Description = item.Description,
                BranchId = item.BranchId
            };

            var result = await _operationTypeStoreService.InsertAndSaveAsync<OperationTypeOutputModel>(operationTypeItem);

            item.Id = result.Result.Id;

            if (item.Equipments != null && result.Info.Succeeded)
            {
                await _operationTypeEquipmentStoreService.UpdateOperationTypeEquipmentsAsync(item);
            }

            if (item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                await _operatingRoomOperationTypeStoreService.UpdateOperationTypeOperatingRoomsAsync(item);
            }

            return result;
        }

        /// <summary>
        /// Remove operationType item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("OperationType/DeleteOperationType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperationType(int id)
        {
            return await _operationTypeStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update operationType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationTypeModel</returns>
        [Route("OperationType/UpdateOperationType")]
        [HttpPost]
        public async Task<ResultModel<OperationTypeOutputModel>> UpdateOperationType([FromBody] OperationTypeInputModel item)
        {
            var operationTypeItem = new OperationTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                BranchId = item.BranchId
            };

            var result = await _operationTypeStoreService.UpdateAndSaveAsync<OperationTypeOutputModel>(operationTypeItem);

            if (item.Equipments != null && result.Info.Succeeded)
            {
                result = await _operationTypeEquipmentStoreService.UpdateOperationTypeEquipmentsAsync(item);
            }

            if (item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                result = await _operatingRoomOperationTypeStoreService.UpdateOperationTypeOperatingRoomsAsync(item);
            }

            return result;
        }
    }
}