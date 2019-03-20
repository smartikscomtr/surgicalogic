using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly ISettingStoreService _settingStoreService;
        private readonly ISettingValueStoreService _settingValueStoreService;
        private readonly ISettingDataTypeStoreService _settingDataTypeStoreService;

        public SettingController(ISettingStoreService settingStoreService, ISettingDataTypeStoreService settingDataTypeStoreService, ISettingValueStoreService settingValueStoreService)
        {
            _settingStoreService = settingStoreService;
            _settingDataTypeStoreService = settingDataTypeStoreService;
            _settingValueStoreService = settingValueStoreService;
        }

        /// <summary>
        /// Get setting methode
        /// </summary>
        /// <returns>SettingOutputModel list</returns>
        [Route("Setting/GetSettings")]
        [HttpGet]
        public async Task<ResultModel<SettingOutputModel>> GetSettings(GridInputModel input)
        {
            var t = await _settingStoreService.GetAsync<SettingOutputModel>(input);

            return t;
        }

        [Route("Setting/GetSettingByName")]
        [HttpGet]
        public async Task<object> GetSettingByName(string name)
        {
            var setting = await _settingStoreService.GetByKeyAsync(name);

            switch (setting.SettingDataTypeId)
            {
                case (int)SettingDataTypeNames.Time:
                    return setting.TimeValue;
                case (int)SettingDataTypeNames.Int:
                    return setting.IntValue;
                case (int)SettingDataTypeNames.Double:
                    return setting.DoubleValue;
                default:
                    return setting.StringValue;
            }
        }

        [Route("Setting/GetAllSettings")]
        public async Task<ResultModel<SettingOutputModel>> GetAllSettings()
        {
            return await _settingStoreService.GetAsync<SettingOutputModel>();
        }

        [Route("Setting/GetSettingDataTypes")]
        public async Task<ResultModel<SettingDataTypeOutputModel>> GetSettingDataTypes()
        {
            return await _settingDataTypeStoreService.GetAsync<SettingDataTypeOutputModel>();
        }

        [Route("Setting/GetSettingValues")]
        public async Task<ResultModel<SettingValueModel>> GetSettingValues()
        {
            return await _settingValueStoreService.GetAsync<SettingValueModel>();
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
                IntValue = item.IntValue,
                StringValue = item.StringValue,
                TimeValue = item.TimeValue,
                SettingValueId = item.SettingValueId,
                DoubleValue = item.DoubleValue,
                SettingDataTypeId = item.SettingDataTypeId
            };

            return await _settingStoreService.UpdateAndSaveAsync<SettingOutputModel>(settingItem);
        }
    }
}