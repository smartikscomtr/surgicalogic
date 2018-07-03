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
        public async Task<ResultModel<WorkTypeOutputModel>> GetWorkTypes()
        {
            return await _workTypeStoreService.GetAsync<WorkTypeOutputModel>();
        }

        [Route("WorkType/InsertWorkType")]
        [HttpPost]
        public async Task<ResultModel<WorkTypeModel>> InsertWorkType([FromBody] WorkTypeInputModel item)
        {
            var workTypeItem = new WorkTypeModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _workTypeStoreService.InsertAndSaveAsync(workTypeItem);
        }

        [Route("WorkType/DeleteWorkType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteWorkType(int id)
        {
            return await _workTypeStoreService.DeleteByIdAsync(id);
        }

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