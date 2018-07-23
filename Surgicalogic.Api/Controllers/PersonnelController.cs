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

            var result = await _personnelStoreService.InsertAndSaveAsync<PersonnelOutputModel>(personnelItem);

            if (result.Info.Succeeded)
            {
                await _personnelBranchStoreService.UpdatePersonelBranchAsync(result.Result.Id, new int[] { item.BranchId });
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

            var result = await _personnelBranchStoreService.UpdatePersonelBranchAsync(item.Id, new int[] { item.BranchId }); 

            if (result.Info.Succeeded)
            {
                result = await _personnelStoreService.UpdateAndSaveAsync<PersonnelOutputModel>(personnelItem);
            }

            return result;
        }
    }
}