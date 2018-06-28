using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchStoreService _branchStoreService;

        public BranchController(IBranchStoreService branchStoreService)
        {
            _branchStoreService = branchStoreService;
        }

        [Route("Branch/GetBranchs")]
        [HttpGet]
        public async Task<ResultModel<BranchModel>> GetBranch([FromQuery] StringFilterSortPaginationModel<BranchSorting, BranchFilter> filter = null)
        {
            return await _branchStoreService.GetAsync(filter);
        }

        [Route("Branch/InsertBranch")]
        [HttpPost]
        public async Task<int> InsertBranch([FromBody] BranchInputModel item)
        {
            var branchItem = new BranchModel()
            {
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return await _branchStoreService.InsertAsync(branchItem);
        }

        [Route("Branch/DeleteBranch/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _branchStoreService.DeleteByIdAsync(id);
            return Json(result);
        }

        [Route("Branch/UpdateBranch")]
        [HttpPost]
        public Task UpdateBranch([FromBody] BranchInputModel item)
        {
            var branchItem = new BranchModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };
            return _branchStoreService.UpdateAsync(branchItem);
        }
    }
}