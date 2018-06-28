using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using System;
using System.Threading.Tasks;

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

        [Route("EquipmentType/InsertEquipmentType")]
        [HttpPost]
        public async Task<int> InsertEquipment([FromBody] EquipmentInputModel item)
        {
            var equipmentItem = new EquipmentModel()
            {
                Name = item.Name,
                EquipmentTypeId = item.EquipmentTypeId,
                IsPortable = item.IsPortable,
                Description = item.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = 2
            };

            return await _equipmentStoreService.InsertAsync(equipmentItem);
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