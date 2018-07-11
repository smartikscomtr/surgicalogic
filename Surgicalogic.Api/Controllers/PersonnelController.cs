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
    public class PersonnelController : Controller
    {
        private readonly IPersonnelStoreService _personnelStoreService;

        public PersonnelController(IPersonnelStoreService personnelStoreService)
        {
            _personnelStoreService = personnelStoreService;
        }

        /// <summary>
        /// Get personnel methode
        /// </summary>
        /// <returns>PersonnelOutputModel list</returns>
        [Route("Personnel/GetPersonnels")]
        [HttpGet]
        public async Task<ResultModel<PersonnelOutputModel>> GetPersonnel()
        {
            return await _personnelStoreService.GetAsync<PersonnelOutputModel>();
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
            var personnelItem = new PersonnelModel()
            {
                PersonnelCode = item.PersonnelCode,
                FirstName = item.FirstName,
                LastName = item.LastName,
                PersonnelTitleId = item.PersonnelTitleId,
                BranchId = item.BranchId,
                WorkTypeId = item.WorkTypeId
            };

            return await _personnelStoreService.InsertAndSaveAsync<PersonnelOutputModel>(personnelItem);
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
            return await _personnelStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update personnel methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelModel</returns>
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