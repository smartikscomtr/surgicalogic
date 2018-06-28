using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    public class WorkTypeController : Controller
    {
        private readonly IWorkTypeStoreService _workTypeStoreService;

        public WorkTypeController(IWorkTypeStoreService workTypeStoreService)
        {
            _workTypeStoreService = workTypeStoreService;
        }

        [Route("WorkType/GetWorkTypes")]
        [HttpGet]
        public async Task<ResultModel<WorkTypeModel>> GetWorkTypes([FromQuery] StringFilterSortPaginationModel<WorkTypeSorting, WorkTypeFilter> filter = null)
        {
            return await _workTypeStoreService.GetAsync(filter);
        }

        [Route("WorkType/InsertWorkType")]
        [HttpPost]
        public async Task<int> InsertWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return await _workTypeStoreService.InsertAsync(workTypeItem);
        }

        [Route("WorkType/DeleteWorkType/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteWorkType(int id)
        {
            var result = await _workTypeStoreService.DeleteByIdAsync(id);

            return Json(result);
        }

        [Route("WorkType/UpdateWorkType")]
        [HttpPost]
        public Task UpdateWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return _workTypeStoreService.UpdateAsync(workTypeItem);
        }
    }
}