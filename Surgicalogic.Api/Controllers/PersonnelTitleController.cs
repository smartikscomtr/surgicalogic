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
    public class PersonnelTitleController : Controller
    {
        private readonly IPersonnelTitleStoreService _personnelTitleStoreService;

        public PersonnelTitleController(IPersonnelTitleStoreService personnelTitleStoreService)
        {
            _personnelTitleStoreService = personnelTitleStoreService;
        }

        /// <summary>
        /// Get personnelTitle methode
        /// </summary>
        /// <returns>PersonnelTitleOutputModel list</returns>
        [Route("PersonnelTitle/GetPersonnelTitles")]
        [HttpGet]
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetPersonnelTitle(GridInputModel input)
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>(input);
        }

        [Route("PersonnelTitle/GetAllPersonnelTitles")]
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetAllPersonnelTitles()
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>();
        }

        [Route("PersonnelTitle/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("PersonnelTitles_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _personnelTitleStoreService.GetExportAsync<PersonnelTitleExportModel>();

            excelService.Write(fs, "Worksheet", typeof(PersonnelTitleExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add personnelTitle methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelTitleOutputModel</returns>
        [Route("PersonnelTitle/InsertPersonnelTitle")]
        [HttpPost]
        public async Task<ResultModel<PersonnelTitleOutputModel>> InsertPersonnelTitle([FromBody] PersonnelTitleInputModel item)
        {
            var personnelTitleItem = new PersonnelTitleModel()
            {
                Name = item.Name,
                SuitableForMultipleOperation = item.SuitableForMultipleOperation,
                Description = item.Description
            };

            return await _personnelTitleStoreService.InsertAndSaveAsync<PersonnelTitleOutputModel>(personnelTitleItem);
        }

        /// <summary>
        /// Remove personnelTitle item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("PersonnelTitle/DeletePersonnelTitle/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePersonnelTitle(int id)
        {
            return await _personnelTitleStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update personnelTitle methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelTitleModel</returns>
        [Route("PersonnelTitle/UpdatePersonnelTitle")]
        [HttpPost]
        public async Task<ResultModel<PersonnelTitleOutputModel>> UpdatePersonnelTitle([FromBody] PersonnelTitleInputModel item)
        {
            var personnelTitleItem = new PersonnelTitleModel()
            {
                Id = item.Id,
                Name = item.Name,
                SuitableForMultipleOperation = item.SuitableForMultipleOperation,
                Description = item.Description
            };

            return await _personnelTitleStoreService.UpdateAndSaveAsync<PersonnelTitleOutputModel>(personnelTitleItem);
        }
    }
}