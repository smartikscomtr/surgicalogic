using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
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
    public class WorkTypeController : Controller
    {
        private readonly IWorkTypeStoreService _workTypeStoreService;

        public WorkTypeController(IWorkTypeStoreService workTypeStoreService)
        {
            _workTypeStoreService = workTypeStoreService;
        }

        /// <summary>
        /// Get workType methode
        /// </summary>
        /// <returns>WorkTypeOutputModel list</returns>
        [Route("WorkType/GetWorkTypes")]
        [HttpGet]
        public async Task<ResultModel<WorkTypeOutputModel>> GetWorkTypes(GridInputModel input)
        {
            return await _workTypeStoreService.GetAsync<WorkTypeOutputModel>(input);
        }

        [Route("WorkType/GetAllWorkTypes")]
        public async Task<ResultModel<WorkTypeOutputModel>> GetAllWorkTypes()
        {
            return await _workTypeStoreService.GetAsync<WorkTypeOutputModel>();
        }

        [Route("WorkType/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("WorkTypes_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _workTypeStoreService.GetExportAsync<WorkTypeExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(WorkTypeExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add workType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>WorkTypeOutputModel</returns>
        [Route("WorkType/InsertWorkType")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeOutputModel>> InsertWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _workTypeStoreService.InsertAndSaveAsync<WorkTypeOutputModel>(workTypeItem);
        }

        /// <summary>
        /// Remove workType item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("WorkType/DeleteWorkType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteWorkType(int id)
        {
            return await _workTypeStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update workType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>WorkTypeModel</returns>
        [Route("WorkType/UpdateWorkType")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeOutputModel>> UpdateWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };

            return await _workTypeStoreService.UpdateAndSaveAsync<WorkTypeOutputModel>(workTypeItem);
        }
    }
}