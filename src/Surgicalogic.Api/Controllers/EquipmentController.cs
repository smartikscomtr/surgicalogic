using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentStoreService _equipmentStoreService;
        private readonly IOperatingRoomEquipmentStoreService _operatingRoomEquipmentStoreService;

        public EquipmentController(IEquipmentStoreService equipmentStoreService, IOperatingRoomEquipmentStoreService operatingRoomEquipmentStoreService)
        {
            _equipmentStoreService = equipmentStoreService;
            _operatingRoomEquipmentStoreService = operatingRoomEquipmentStoreService;
        }

        /// <summary>
        /// Get equipment methode
        /// </summary>
        /// <returns>EquipmentOutputModel list</returns>
        [Route("Equipment/GetEquipments")]
        [HttpGet]
        public async Task<ResultModel<EquipmentOutputModel>> GetEquipments(GridInputModel item)
        {
            return await _equipmentStoreService.GetAsync<EquipmentOutputModel>(item);
        }

        [Route("Equipment/GetAllEquipments")]
        public async Task<ResultModel<EquipmentOutputModel>> GetAllEquipments()
        {
            return await _equipmentStoreService.GetAsync<EquipmentOutputModel>();
        }

        [Route("Equipment/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Equipments_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _equipmentStoreService.GetExportAsync<EquipmentExportModel>();

            excelService.Write(fs, "Worksheet", typeof(EquipmentExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [Route("Equipment/GetNonPortableEquipments")]
        public async Task<ResultModel<EquipmentOutputModel>> GetNonPortableEquipments()
        {
            return await _equipmentStoreService.GetNonPortableEquipments();
        }

        /// <summary>
        /// Add equipment methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentOutputModel</returns>
        [Route("Equipment/InsertEquipment")]
        [HttpPost]
        public async Task<ResultModel<EquipmentOutputModel>> InsertEquipment([FromBody] EquipmentInputModel item)
        {
            var result = new ResultModel<EquipmentOutputModel>();

            var isDuplicateCode = await _equipmentStoreService.IsDuplicateCode(item.Code, item.Id);

            if (isDuplicateCode)
            {
                result.Info = new Info
                {
                    Succeeded = false,
                    InfoType = Model.Enum.InfoType.Error,
                    Message = Model.Enum.MessageType.CodeIsNotUnique
                };

                return result;
            }

            var equipmentItem = new EquipmentModel()
            {
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId
            };

            result = await _equipmentStoreService.InsertAndSaveAsync<EquipmentOutputModel>(equipmentItem);

            if (!item.IsPortable && item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                item.Id = result.Result.Id;
                await _operatingRoomEquipmentStoreService.UpdateEquipmentOperatingRoomsAsync(item);
            }

            return result;
        }

        /// <summary>
        /// Remove equipment item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Equipment/DeleteEquipment/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteEquipmentType(int id)
        {
            var result = await _equipmentStoreService.DeleteAndSaveByIdAsync(id);

            if (result.Result > 0)
            {
                await _operatingRoomEquipmentStoreService.DeleteByOperatingRoomIdAsync(id);
            }

            return result;
        }

        /// <summary>
        /// Update equipment methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentModel</returns>
        [Route("Equipment/UpdateEquipment")]
        [HttpPost]
        public async Task<ResultModel<EquipmentOutputModel>> UpdateEquipments([FromBody] EquipmentInputModel item)
        {
            var result = new ResultModel<EquipmentOutputModel>();

            var isDuplicateCode = await _equipmentStoreService.IsDuplicateCode(item.Code, item.Id);

            if (isDuplicateCode)
            {
                result.Info = new Info
                {
                    Succeeded = false,
                    InfoType = Model.Enum.InfoType.Error,
                    Message = Model.Enum.MessageType.CodeIsNotUnique
                };

                return result;
            }

            var equipmentItem = new EquipmentModel()
            {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                IsPortable = item.IsPortable,
                EquipmentTypeId = item.EquipmentTypeId
            };

            result = await _equipmentStoreService.UpdateAndSaveAsync<EquipmentOutputModel>(equipmentItem);

            if (!item.IsPortable && item.OperatingRoomIds != null && result.Info.Succeeded)
            {
                await _operatingRoomEquipmentStoreService.UpdateEquipmentOperatingRoomsAsync(item);
            }

            return result;
        }
    }
}