using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    public class SettingController : Controller
    {
        private readonly IWorkTypeStoreService _settingStoreService;

        public SettingController(IWorkTypeStoreService settingStoreService)
        {
            _settingStoreService = settingStoreService;
        }

        /// <summary>
        /// Get setting methode
        /// </summary>
        /// <returns>SettingOutputModel list</returns>
        [Route("Setting/GetSettings")]
        [HttpGet]
        public async Task<ResultModel<SettingOutputModel>> GetSettings(GridInputModel input)
        {
            return await _settingStoreService.GetAsync<SettingOutputModel>(input);
        }

        [Route("Setting/GetAllSettings")]
        public async Task<ResultModel<SettingOutputModel>> GetAllSettings()
        {
            return await _settingStoreService.GetAsync<SettingOutputModel>();
        }

        /// <summary>
        /// Update setting methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>SettingModel</returns>
        [Route("Setting/UpdateSetting")]
        [HttpPost]
        public async Task<ResultModel<SettingOutputModel>> UpdateSetting([FromBody] SettingInputModel item)
        {
            var settingItem = new SettingModel()
            {
                Id = item.Id,
                Name = item.Name,
                Value = item.Value
            };

            return new ResultModel<SettingOutputModel>();// await _settingStoreService.UpdateAndSaveAsync<SettingOutputModel>(settingItem);
        }
    }
}