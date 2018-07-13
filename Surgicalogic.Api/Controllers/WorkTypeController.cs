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
    public class WorkTypeController : Controller
    {
        private readonly IWorkTypeStoreService _workTypeStoreService;

        public WorkTypeController(IWorkTypeStoreService workTypeStoreService)
        {
            _workTypeStoreService = workTypeStoreService;
        }

        /// <summary>
        /// Get workType methode
        /// </summary>
        /// <returns>WorkTypeOutputModel list</returns>
        [Route("WorkType/GetWorkTypes")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeOutputModel>> GetWorkTypes([FromBody]GridInputModel input)
        {
            return await _workTypeStoreService.GetAsync<WorkTypeOutputModel>(input);
        }

        /// <summary>
        /// Add workType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>WorkTypeOutputModel</returns>
        [Route("WorkType/InsertWorkType")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeOutputModel>> InsertWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _workTypeStoreService.InsertAndSaveAsync<WorkTypeOutputModel>(workTypeItem);
        }

        /// <summary>
        /// Remove workType item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("WorkType/DeleteWorkType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteWorkType(int id)
        {
            return await _workTypeStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update workType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>WorkTypeModel</returns>
        [Route("WorkType/UpdateWorkType")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeModel>> UpdateWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };

            return await _workTypeStoreService.UpdatandSaveAsync(workTypeItem);
        }
    }
}