using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
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
        public async Task<ResultModel<WorkTypeOutputModel>> GetWorkTypes([FromQuery] StringFilterSortPaginationModel<WorkTypeSorting, WorkTypeFilter> filter = null)
        {
            return await _workTypeStoreService.GetAsync<WorkTypeOutputModel>(filter);
        }

        [Route("WorkType/InsertWorkType")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeModel>> InsertWorkType([FromBody] WorkTypeInputModel item)
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
        public async Task<ResultModel<int>> DeleteWorkType(int id)
        {
            return await _workTypeStoreService.DeleteByIdAsync(id);
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
                ModifiedDate = DateTime.Now,
                ModifiedBy = 2
            };

            return _workTypeStoreService.UpdateAsync(workTypeItem);
        }
    }
}