using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System;
using System.Threading.Tasks;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class PersonnelController : Controller
    {
        private readonly IPersonnelStoreService _personnelStoreService;

        public PersonnelController(IPersonnelStoreService personnelStoreService)
        {
            _personnelStoreService = personnelStoreService;
        }

        // GET api/values
        [Route("Personnel/GetPersonnels")]
        [HttpGet]
        public async Task<ResultModel<PersonnelOutputModel>> GetPersonnel()
        {
            return await _personnelStoreService.GetAsync<PersonnelOutputModel>();
        }

        [Route("Personnel/InsertPersonnel")]
        [HttpPost]
        public async Task<ResultModel<PersonnelModel>> InsertPersonnel([FromBody] PersonnelInputModel item)
        {
            var personnelItem = new PersonnelModel()
            {
                PersonnelCode = item.PersonnelCode,
                FirstName = item.FirstName,
                LastName = item.LastName,
                PersonnelTitleId = item.PersonnelTitleId,
                BranchId = item.BranchId,
                WorkTypeId = item.WorkTypeId
            };

            return await _personnelStoreService.InsertAndSaveAsync(personnelItem);
        }

        [Route("Personnel/DeletePersonnel/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePersonnel(int id)
        {
            return await _personnelStoreService.DeleteByIdAsync(id);
        }

        [Route("Personnel/UpdatePersonnel")]
        [HttpPost]
        public async Task<ResultModel<PersonnelModel>> UpdatePersonnel([FromBody] PersonnelInputModel item)
        {
            var personnelItem = new PersonnelModel()
            {
                Id = item.Id,
                PersonnelCode = item.PersonnelCode,
                FirstName = item.FirstName,
                LastName = item.LastName,
                PersonnelTitleId = item.PersonnelTitleId,
                BranchId = item.BranchId,
                WorkTypeId = item.WorkTypeId
            };

            return await _personnelStoreService.UpdatandSaveAsync(personnelItem);
        }
    }
}