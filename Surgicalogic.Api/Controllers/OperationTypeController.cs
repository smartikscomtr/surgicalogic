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
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class OperationTypeController : Controller
    {
        private readonly IOperationTypeStoreService _operationTypeStoreService;

        public OperationTypeController(IOperationTypeStoreService operationTypeStoreService)
        {
            _operationTypeStoreService = operationTypeStoreService;
        }

        /// <summary>
        /// Get operationType methode
        /// </summary>
        /// <returns>OperationTypeOutputModel list</returns>
        [Route("OperationType/GetOperationTypes")]
        [HttpPost]
        public async Task<ResultModel<OperationTypeOutputModel>> GetOperationTypes([FromBody]GridInputModel input)
        {
            return await _operationTypeStoreService.GetAsync<OperationTypeOutputModel>(input);
        }

        /// <summary>
        /// Add operationType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationTypeOutputModel</returns>
        [Route("OperationType/InsertOperationType")]
        public async Task<ResultModel<OperationTypeOutputModel>> InsertOperationType([FromBody] OperationTypeInputModel item)
        {
            var operationTypeItem = new OperationTypeModel()
            {
                Name = item.Name,
                Description = item.Description,
                //Branch = item.BranchId
            };

            return await _operationTypeStoreService.InsertAndSaveAsync<OperationTypeOutputModel>(operationTypeItem);
        }

        /// <summary>
        /// Remove operationType item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("OperationType/DeleteOperationType/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperationType(int id)
        {
            return await _operationTypeStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update operationType methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationTypeModel</returns>
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