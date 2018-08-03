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
    public class OperationController : Controller
    {
        private readonly IOperationStoreService _operationStoreService;

        public OperationController(IOperationStoreService operationStoreService)
        {
            _operationStoreService = operationStoreService;
        }

        /// <summary>
        /// Get Operation methode
        /// </summary>
        /// <returns>OperationOutputModel list</returns>
        [Route("Operation/GetOperations")]
        [HttpGet]
        public async Task<ResultModel<OperationOutputModel>> GetOperations(GridInputModel input)
        {
            return await _operationStoreService.GetAsync<OperationOutputModel>(input);
        }

        [Route("Operation/GetAllOperations")]
        public async Task<ResultModel<OperationOutputModel>> GetAllOperations()
        {
            return await _operationStoreService.GetAsync<OperationOutputModel>();
        }

        /// <summary>
        /// Add Operation methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationOutputModel</returns>
        [Route("Operation/InsertOperation")]
        [HttpPost]
        public async Task<ResultModel<OperationOutputModel>> InsertOperation([FromBody] OperationInputModel item)
        {
            var operationItem = new OperationModel()
            {
                Name = item.Name,
                Description = item.Description,
                OperationTypeId = item.OperationTypeId,
                Date = item.Date
            };

            return await _operationStoreService.InsertAndSaveAsync<OperationOutputModel>(operationItem);
        }

        /// <summary>
        /// Remove Operation item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Operation/DeleteOperation/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteOperation(int id)
        {
            return await _operationStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update Operation methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>OperationModel</returns>
        [Route("Operation/UpdateOperation")]
        [HttpPost]
        public async Task<ResultModel<OperationOutputModel>> UpdateOperation([FromBody] OperationInputModel item)
        {
            var operationItem = new OperationModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                OperationTypeId = item.OperationTypeId,
                Date = item.Date
            };

            return await _operationStoreService.UpdateAndSaveAsync<OperationOutputModel>(operationItem);
        }
    }
}