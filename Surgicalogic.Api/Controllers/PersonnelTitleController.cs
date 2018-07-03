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
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetPersonnelTitle ([FromQuery] StringFilterSortPaginationModel <PersonnelTitleSorting, PersonnelTitleFilter> filter = null)
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>(filter);
        }

        [Route("PersonnelTitle/InsertPersonnelTitle")]
        [HttpPost]
        public async Task<ResultModel<PersonnelTitleModel>> InsertPersonnelTitle([FromBody] PersonnelTitleInputModel item)
        {
            var personnelTitleItem = new PersonnelTitleModel()
            {
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return await _personnelTitleStoreService.InsertAsync(personnelTitleItem);
        }

        [Route("PersonnelTitle/DeletePersonnelTitle/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeletePersonnelTitle(int id)
        {
            return await _personnelTitleStoreService.DeleteByIdAsync(id);
        }

        [Route("PersonnelTitle/UpdatePersonnelTitle")]
        [HttpPost]
        public Task<ResultModel<PersonnelTitleModel>> UpdatePersonnelTitle([FromBody] PersonnelTitleInputModel item)
        {
            var personnelTitleItem = new PersonnelTitleModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ModifiedDate = DateTime.Now,
                ModifiedBy = 2
            };

            return _personnelTitleStoreService.UpdateAsync(personnelTitleItem);
        }
    }
}