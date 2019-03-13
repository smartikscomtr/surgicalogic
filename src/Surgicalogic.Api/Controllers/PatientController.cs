using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientStoreService _patientStoreService;

        public PatientController(IPatientStoreService patientStoreService)
        {
            _patientStoreService = patientStoreService;
        }

        /// <summary>
        /// Get patient methode
        /// </summary>
        /// <returns>PatientOutputModel list</returns>
        [Route("Patient/GetPatients")]
        [HttpGet]
        public async Task<ResultModel<PatientOutputModel>> GetPatient(GridInputModel input)
        {
            return await _patientStoreService.GetAsync<PatientOutputModel>(input);
        }

        [Route("Patient/GetAllPatients")]
        public async Task<ResultModel<PatientOutputModel>> GetAllPatients()
        {
            return await _patientStoreService.GetAsync<PatientOutputModel>();
        }

        [Route("Patient/GetAllPatientsForOperation")]
        public async Task<ResultModel<PatientForOperationOutputModel>> GetAllPatientsForOperation()
        {
            return await _patientStoreService.GetAsync<PatientForOperationOutputModel>();
        }

        [Route("Patient/ExcelExport")]
        public async Task<string> ExcelExport(string langId)
        {
            AppSettings.SetSiteLanguage(langId);

            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Patients_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _patientStoreService.GetExportAsync<PatientExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(PatientExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add patient methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PatientOutputModel</returns>
        [Route("Patient/InsertPatient")]
        [HttpPost]
        public async Task<ResultModel<PatientOutputModel>> InsertPatient([FromBody] PatientInputModel item)
        {
            var patientItem = new PatientModel()
            {
                IdentityNumber = item.IdentityNumber,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Phone = item.Phone,
                Address = item.Address
            };

            return await _patientStoreService.InsertAndSaveAsync<PatientOutputModel>(patientItem);
        }

        /// <summary>
        /// Remove patient item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Patient/DeletePatient/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePatient(int id)
        {
            return await _patientStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update patient methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PatientModel</returns>
        [Route("Patient/UpdatePatient")]
        [HttpPost]
        public async Task<ResultModel<PatientOutputModel>> UpdatePatient([FromBody] PatientInputModel item)
        {
            var patientModel = new PatientModel()
            {
                Id = item.Id,
                IdentityNumber = item.IdentityNumber,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Phone = item.Phone,
                Address = item.Address
            };

            return await _patientStoreService.UpdateAndSaveAsync<PatientOutputModel>(patientModel);
        }
    }
}