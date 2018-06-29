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
    public class BranchController : Controller
    {
        private readonly IBranchStoreService _branchStoreService;

        public BranchController(IBranchStoreService branchStoreService)
        {
            _branchStoreService = branchStoreService;
        }

        [Route("Branch/GetBranchs")]
        [HttpGet]
        public async Task<ResultModel<BranchOutputModel>> GetBranch([FromQuery] StringFilterSortPaginationModel<BranchSorting, BranchFilter> filter = null)
        {
            return await _branchStoreService.GetAsync<BranchOutputModel>(filter);
        }

        [Route("Branch/InsertBranch")]
        [HttpPost]
        public async Task<ResultModel<BranchModel>> InsertBranch([FromBody] BranchInputModel item)
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
        public async Task<ResultModel<int>> DeleteBranch(int id)
        {            
            return await _branchStoreService.DeleteByIdAsync(id);
        }

        [Route("Branch/UpdateBranch")]
        [HttpPost]
        public Task<ResultModel<BranchModel>> UpdateBranch([FromBody] BranchInputModel item)
        {
            var branchItem = new BranchModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ModifiedDate = DateTime.Now,
                ModifiedBy = 2
            };
            return _branchStoreService.UpdateAsync(branchItem);
        }
    }
}