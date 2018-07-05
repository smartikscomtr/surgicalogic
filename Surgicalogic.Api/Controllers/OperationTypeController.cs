using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class OperationTypeController : Controller
    {
        private readonly IOperationTypeStoreService _operationTypeStoreService;

        public OperationTypeController(IOperationTypeStoreService operationTypeStoreService)
        {
            _operationTypeStoreService = operationTypeStoreService;
        }

        public async Task<ResultModel<OperationTypeOutputModel>> GetOperationTypes()
        {
            return await _operationTypeStoreService.GetAsync<OperationTypeOutputModel>();
        }

        public async Task<ResultModel<OperationTypeModel>> InsertOperationType([FromBody] OperationTypeInputModel item)
        {
            var operationTypeItem = new OperationTypeModel()
            {
                Name = item.Name,
                Description = item.Description,
                //Branch = item.BranchId
            };

            return await _operationTypeStoreService.InsertAndSaveAsync(operationTypeItem);
        }

        [Route("OperationType/DeleteOperationType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperationType(int id)
        {
            return await _operationTypeStoreService.DeleteByIdAsync(id);
        }

        [Route("OperationType/UpdateOperationType")]
        [HttpPost]
        public async Task<ResultModel<OperationTypeModel>> UpdateOperationType([FromBody] OperationTypeInputModel item)
        {
            var operationTypeItem = new OperationTypeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                //Branch = item.BranchId
            };

            return await _operationTypeStoreService.UpdatandSaveAsync(operationTypeItem);
        }
    }
}