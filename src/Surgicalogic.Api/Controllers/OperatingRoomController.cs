﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    [Authorize]
    public class OperatingRoomController : Controller
    {
        private readonly IOperatingRoomStoreService _operatingRoomStoreService;
        private readonly IOperatingRoomEquipmentStoreService _operatingRoomEquipmentStoreService;

        public OperatingRoomController(IOperatingRoomStoreService operatingRoomStoreService, IOperatingRoomEquipmentStoreService operatingRoomEquipmentStoreService)
        {
            _operatingRoomStoreService = operatingRoomStoreService;
            _operatingRoomEquipmentStoreService = operatingRoomEquipmentStoreService;
        }

        /// <summary>
        /// Get operatingRoom methode
        /// </summary>
        /// <returns>OperatingRoomOutputModel list</returns>
        [Route("OperatingRoom/GetOperatingRooms")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomOutputModel>> GetOperatingRooms(GridInputModel input)
        {
            return await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>(input);
        }

        [Route("OperatingRoom/GetAllOperatingRooms")]
        [HttpGet]
        public async Task<ResultModel<OperatingRoomOutputModel>> GetAllOperatingRooms()
        {
            return await _operatingRoomStoreService.GetAsync<OperatingRoomOutputModel>();
        }


        [Route("OperatingRoom/ExcelExport")]
        public async Task<string> ExcelExport(string langId)
        {
            AppSettings.SetSiteLanguage(langId);

            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("OperatingRooms_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _operatingRoomStoreService.GetExportAsync<OperatingRoomExportModel>();

           await excelService.WriteAsync(fs, "Worksheet", items, typeof(OperatingRoomExportModel), System.Globalization.CultureInfo.CurrentCulture);

           return fileName;
        }

        /// <summary>
        /// Add operatingRoom methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperatingRoomOutputModel</returns>
        [Route("OperatingRoom/InsertOperatingRoom")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomOutputModel>> InsertOperatingRoom([FromBody] OperatingRoomInputModel item)
        {
            var model = new ResultModel<OperatingRoomOutputModel>();

            var operatingRoomItem = new OperatingRoomModel()
            {
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length,
                IsAvailable = true
            };

            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                model = await _operatingRoomStoreService.InsertAndSaveAsync<OperatingRoomOutputModel>(operatingRoomItem);

                item.Id = model.Result.Id;

                if (model.Info.Succeeded && item.Equipments != null && item.Equipments.Count > 0)
                {
                    var result = await _operatingRoomStoreService.UpdateOperatingRoomEquipmentsAsync(item);

                    if (!result.Info.Succeeded)
                    {
                        return result;
                    }
                }

                if (model.Info.Succeeded && item.OperationTypes != null && item.OperationTypes.Count > 0)
                {
                    var result = await _operatingRoomStoreService.UpdateOperatingRoomOperationTypesAsync(item);

                    if (!result.Info.Succeeded)
                    {
                        return result;
                    }
                }

                ts.Complete();
            }

            return model;
        }

        /// <summary>
        /// Remove operatingRoom item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("OperatingRoom/DeleteOperatingRoom/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperatingRoom(int id)
        {
            var hasOperations = await _operatingRoomStoreService.HasOperation(id);
            
            if (hasOperations)
            {
                return new ResultModel<int>
                {
                    Info = new Info
                    {
                        Succeeded = false,
                        InfoType = Model.Enum.InfoType.Warning,
                        Message = Model.Enum.MessageType.OperatingRoomHasOperation
                    }
                };
            }

            return await _operatingRoomStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update operatingRoom methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperatingRoomModel</returns>
        [Route("OperatingRoom/UpdateOperatingRoom")]
        [HttpPost]
        public async Task<ResultModel<OperatingRoomOutputModel>> UpdateOperatingRoom([FromBody] OperatingRoomInputModel item)
        {
            var result = new ResultModel<OperatingRoomOutputModel>();
            var model = new OperatingRoomModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                IsAvailable = true,
                Length = item.Length
            };

            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                if (item.Equipments != null)
                {
                    result = await _operatingRoomStoreService.UpdateOperatingRoomEquipmentsAsync(item);

                    if (!result.Info.Succeeded)
                    {
                        return result;
                    }
                }

                if (item.OperationTypes != null)
                {
                    result = await _operatingRoomStoreService.UpdateOperatingRoomOperationTypesAsync(item);

                    if (!result.Info.Succeeded)
                    {
                        return result;
                    }
                }

                result = await _operatingRoomStoreService.UpdateAndSaveAsync<OperatingRoomOutputModel>(model);
                ts.Complete();
            }

            return result;
        }

        [Route("OperatingRoom/GetOperatingRoomsByOperationTypeId/{operationTypeId:int}")]
        [HttpGet]
        public async Task<List<OperatingRoomOutputModel>> GetPersonnelsByBranchId(int operationTypeId)
        {
            return  await _operatingRoomStoreService.GetByOperationTypeIdAsync(operationTypeId);
        }
    }
}