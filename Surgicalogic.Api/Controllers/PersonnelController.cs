using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
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
    public class PersonnelController : Controller
    {
        private readonly IPersonnelStoreService _personnelStoreService;
        private readonly IPersonnelBranchStoreService _personnelBranchStoreService;

        public PersonnelController(IPersonnelStoreService personnelStoreService, IPersonnelBranchStoreService personnelBranchStoreService)
        {
            _personnelStoreService = personnelStoreService;
            _personnelBranchStoreService = personnelBranchStoreService;
        }

        /// <summary>
        /// Get personnel methode
        /// </summary>
        /// <returns>PersonnelOutputModel list</returns>
        [Route("Personnel/GetPersonnels")]
        [HttpGet]
        public async Task<ResultModel<PersonnelOutputModel>> GetPersonnel(GridInputModel input)
        {
            return await _personnelStoreService.GetAsync<PersonnelOutputModel>(input);
        }

        [Route("Personnel/GetAllPersonnels")]
        public async Task<ResultModel<PersonnelOutputModel>> GetAllPersonnels()
        {
            return await _personnelStoreService.GetAsync<PersonnelOutputModel>();
        }

        [Route("Personnel/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Personnels_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _personnelStoreService.GetExportAsync<PersonnelExportModel>();

            excelService.Write(fs, "Worksheet", typeof(PersonnelExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        [Route("Personnel/GetPersonnelsByBranchIdAsync/{branchId:int}")]
        [HttpGet]
        public async Task<List<PersonnelOutputModel>> GetPersonnelsByBranchIdAsync(int branchId)
        {
            return await _personnelStoreService.GetPersonnelsByBranchIdAsync(branchId);
        }

        [Route("Personnel/GetPersonnelsByOperationTypeIdAsync/{operationTypeId:int}")]
        [HttpGet]
        public async Task<List<PersonnelOutputModel>> GetPersonnelsByOperationTypeIdAsync(int operationTypeId)
        {
            return await _personnelStoreService.GetPersonnelsByOperationTypeIdAsync(operationTypeId);
        }

        [Route("Personnel/GetDoctorsByBranchIdAsync/{branchId:int}")]
        [HttpGet]
        public async Task<List<PersonnelOutputModel>> GetDoctorsByBranchIdAsync(int branchId)
        {
            return await _personnelStoreService.GetDoctorsByBranchIdAsync(branchId);
        }

        /// <summary>
        /// Add personnel methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelOutputModel</returns>
        [Route("Personnel/InsertPersonnel")]
        [HttpPost]
        public async Task<ResultModel<PersonnelOutputModel>> InsertPersonnel([FromBody] PersonnelInputModel item)
        {
            var result = new ResultModel<PersonnelOutputModel>();
            var personnelItem = new PersonnelModel()
            {
                PersonnelCode = item.PersonnelCode,
                FirstName = item.FirstName,
                LastName = item.LastName,
                PersonnelCategoryId = item.PersonnelCategoryId,
                PersonnelTitleId = item.PersonnelTitleId,
                BranchId = 1,
                WorkTypeId = item.WorkTypeId
            };

            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _personnelStoreService.InsertAndSaveAsync<PersonnelOutputModel>(personnelItem);

                if (item.Branches != null && result.Info.Succeeded)
                {
                    await _personnelBranchStoreService.UpdatePersonelBranchAsync(result.Result.Id, item.Branches.ToArray());
                }

                ts.Complete();
            }

            return result;
        }

        /// <summary>
        /// Remove personnel item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Personnel/DeletePersonnel/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePersonnel(int id)
        {
            return await _personnelStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update personnel methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelModel</returns>
        [Route("Personnel/UpdatePersonnel")]
        [HttpPost]
        public async Task<ResultModel<PersonnelOutputModel>> UpdatePersonnel([FromBody] PersonnelInputModel item)
        {
            var result = new ResultModel<PersonnelOutputModel>() { Info = new Info() };

            var personnelItem = new PersonnelModel()
            {
                Id = item.Id,
                PersonnelCode = item.PersonnelCode,
                FirstName = item.FirstName,
                LastName = item.LastName,
                PersonnelCategoryId = item.PersonnelCategoryId,
                PersonnelTitleId = item.PersonnelTitleId,
                BranchId = 1,
                WorkTypeId = item.WorkTypeId
            };

            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                if (item.Branches != null)
                {
                    result = await _personnelBranchStoreService.UpdatePersonelBranchAsync(item.Id, item.Branches.ToArray());
                }

                if (result.Info.Succeeded)
                {
                    result = await _personnelStoreService.UpdateAndSaveAsync<PersonnelOutputModel>(personnelItem);
                }

                ts.Complete();
            }

            return result;
        }
    }
}