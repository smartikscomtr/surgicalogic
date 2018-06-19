using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.StoreServices;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Equipment")]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentStoreService _equipmentStoreService;

        public EquipmentController(IEquipmentStoreService equipmentStoreService)
        {
            _equipmentStoreService = equipmentStoreService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ResultModel<EquipmentModel>> GetEquipments([FromQuery] StringFilterSortPaginationModel<EquipmentSorting, EquipmentFilter> filter = null)
        {
            return await _equipmentStoreService.GetAsync(filter);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
    }
}