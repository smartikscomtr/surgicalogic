using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentStoreService _equipmentStoreService;

        public EquipmentController(IEquipmentStoreService equipmentStoreService)
        {
            _equipmentStoreService = equipmentStoreService;
        }

        // GET api/values
        [Route("Equipment/GetEquipments")]
        [HttpGet]
        public async Task<ResultModel<EquipmentModel>> GetEquipments([FromQuery] StringFilterSortPaginationModel<EquipmentSorting, EquipmentFilter> filter = null)
        {
            return await _equipmentStoreService.GetAsync(filter);          
        }

        [Route("Equipment/UpdateEquipment")]
        [HttpPost("")]
        public Task UpdateEquipments([FromQuery] StringFilterSortPaginationModel<EquipmentSorting, EquipmentFilter> filter = null)
        {
            var m = new EquipmentModel();
            return _equipmentStoreService.UpdateAsync(m);
        }



        // POST api/values
        //[HttpPost]
        //public async Task AddEquipments([FromBody]string value)
        //{
        //    //await _equipmentStoreService.InsertAsync();
        //}
    }
}