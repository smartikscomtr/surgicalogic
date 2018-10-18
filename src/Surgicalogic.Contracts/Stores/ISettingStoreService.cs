using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface ISettingStoreService : IStoreService<Setting, SettingModel>
    {
        Task<SettingModel> GetByKeyAsync(string key);
        Task<List<SettingModel>> GetAllAsync();
    }
}