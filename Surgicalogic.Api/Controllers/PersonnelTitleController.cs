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
    public class PersonnelTitleController : Controller
    {
        private readonly IPersonnelTitleStoreService _personnelTitleStoreService;

        public PersonnelTitleController (IPersonnelTitleStoreService personnelTitleStoreService)
        {
            _personnelTitleStoreService = personnelTitleStoreService;
        }

        [Route("PersonnelTitle/GetPersonnelTitles")]
        [HttpGet]
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetPersonnelTitle ()
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>();
        }

        [Route("PersonnelTitle/InsertPersonnelTitle")]
        [HttpPost]
        public async Task<ResultModel<PersonnelTitleModel>> InsertPersonnelTitle([FromBody] PersonnelTitleInputModel item)
        {
            var personnelTitleItem = new PersonnelTitleModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _personnelTitleStoreService.InsertAndSaveAsync(personnelTitleItem);
        }

        [Route("PersonnelTitle/DeletePersonnelTitle/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePersonnelTitle(int id)
        {
            return await _personnelTitleStoreService.DeleteByIdAsync(id);
        }

        [Route("PersonnelTitle/UpdatePersonnelTitle")]
        [HttpPost]
        public async Task<ResultModel<PersonnelTitleModel>> UpdatePersonnelTitle([FromBody] PersonnelTitleInputModel item)
        {
            var personnelTitleItem = new PersonnelTitleModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };

            return await _personnelTitleStoreService.UpdatandSaveAsync(personnelTitleItem);
        }
    }
}