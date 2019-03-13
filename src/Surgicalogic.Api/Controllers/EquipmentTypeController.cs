using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Common.Settings;
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
    public class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeStoreService _equipmentTypeStoreService;

        public EquipmentTypeController(IEquipmentTypeStoreService equipmentTypeStoreService)
        {
            _equipmentTypeStoreService = equipmentTypeStoreService;
        }

        /// <summary>
        /// Get equipmentType methode
        /// </summary>
        /// <returns>EquipmentTypeOutputModel list</returns>
        [Route("EquipmentType/GetEquipmentTypes")]
        [HttpGet]
        public async Task<ResultModel<EquipmentTypeOutputModel>> GetEquipmentTypes(GridInputModel input)
        {
            return await _equipmentTypeStoreService.GetAsync<EquipmentTypeOutputModel>(input);
        }

        [Route("EquipmentType/GetAllEquipmentTypes")]
        public async Task<ResultModel<EquipmentTypeOutputModel>> GetAllEquipmentTypes()
        {
            return await _equipmentTypeStoreService.GetAsync<EquipmentTypeOutputModel>();
        }


        [Route("EquipmentType/ExcelExport")]
        public async Task<string> ExcelExport(string langId)
        {
            AppSettings.SetSiteLanguage(langId);

            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("EquipmentTypes_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _equipmentTypeStoreService.GetExportAsync<EquipmentTypeExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(EquipmentTypeExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add equipmentType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentTypeOutputModel</returns>
        [Route("EquipmentType/InsertEquipmentType")]
        [HttpPost]
        public async Task<ResultModel<EquipmentTypeOutputModel>> InsertEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _equipmentTypeStoreService.InsertAndSaveAsync<EquipmentTypeOutputModel>(equipmentTypeItem);
        }

        /// <summary>
        /// Remove equipmentType item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("EquipmentType/DeleteEquipmentType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteEquipmentType(int id)
        {
            return await _equipmentTypeStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update equipmentType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>EquipmentTypeModel</returns>
        [Route("EquipmentType/UpdateEquipmentType")]
        [HttpPost]
        public async Task<ResultModel<EquipmentTypeOutputModel>> UpdateEquipmentType([FromBody] EquipmentTypeInputModel item)
        {
            var equipmentTypeItem = new EquipmentTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };

            return await _equipmentTypeStoreService.UpdateAndSaveAsync<EquipmentTypeOutputModel>(equipmentTypeItem);
        }
    }
}