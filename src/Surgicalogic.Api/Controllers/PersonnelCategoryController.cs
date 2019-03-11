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
    public class PersonnelCategoryController : Controller
    {
        private readonly IPersonnelCategoryStoreService _PersonnelCategoryStoreService;

        public PersonnelCategoryController(IPersonnelCategoryStoreService PersonnelCategoryStoreService)
        {
            _PersonnelCategoryStoreService = PersonnelCategoryStoreService;
        }

        /// <summary>
        /// Get PersonnelCategory methode
        /// </summary>
        /// <returns>PersonnelCategoryOutputModel list</returns>
        [Route("PersonnelCategory/GetPersonnelCategories")]
        [HttpGet]
        public async Task<ResultModel<PersonnelCategoryOutputModel>> GetPersonnelCategory(GridInputModel input)
        {
            return await _PersonnelCategoryStoreService.GetAsync<PersonnelCategoryOutputModel>(input);
        }

        [Route("PersonnelCategory/GetAllPersonnelCategories")]
        public async Task<ResultModel<PersonnelCategoryOutputModel>> GetAllPersonnelCategories()
        {
            return await _PersonnelCategoryStoreService.GetAsync<PersonnelCategoryOutputModel>();
        }

        [Route("PersonnelCategory/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("PersonnelCategories_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _PersonnelCategoryStoreService.GetExportAsync<PersonnelCategoryExportModel>();

            excelService.Write(fs, "Worksheet", typeof(PersonnelCategoryExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add PersonnelCategory methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelCategoryOutputModel</returns>
        [Route("PersonnelCategory/InsertPersonnelCategory")]
        [HttpPost]
        public async Task<ResultModel<PersonnelCategoryOutputModel>> InsertPersonnelCategory([FromBody] PersonnelCategoryInputModel item)
        {
            var PersonnelCategoryItem = new PersonnelCategoryModel()
            {
                Name = item.Name,
                SuitableForMultipleOperation = item.SuitableForMultipleOperation,
                Description = item.Description
            };

            return await _PersonnelCategoryStoreService.InsertAndSaveAsync<PersonnelCategoryOutputModel>(PersonnelCategoryItem);
        }

        /// <summary>
        /// Remove PersonnelCategory item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("PersonnelCategory/DeletePersonnelCategory/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePersonnelCategory(int id)
        {
            return await _PersonnelCategoryStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update PersonnelCategory methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelCategoryModel</returns>
        [Route("PersonnelCategory/UpdatePersonnelCategory")]
        [HttpPost]
        public async Task<ResultModel<PersonnelCategoryOutputModel>> UpdatePersonnelCategory([FromBody] PersonnelCategoryInputModel item)
        {
            var PersonnelCategoryItem = new PersonnelCategoryModel()
            {
                Id = item.Id,
                Name = item.Name,
                SuitableForMultipleOperation = item.SuitableForMultipleOperation,
                Description = item.Description
            };

            return await _PersonnelCategoryStoreService.UpdateAndSaveAsync<PersonnelCategoryOutputModel>(PersonnelCategoryItem);
        }
    }
}