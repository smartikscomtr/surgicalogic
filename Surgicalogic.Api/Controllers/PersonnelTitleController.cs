using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
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
        [HttpPost]
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetPersonnelTitle([FromBody]GridInputModel input)
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>(input);
        }

        [Route("PersonnelTitle/GetAllPersonnelTitles")]
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetAllPersonnelTitles()
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>();
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
            return await _personnelTitleStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update personnelTitle methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelTitleModel</returns>
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