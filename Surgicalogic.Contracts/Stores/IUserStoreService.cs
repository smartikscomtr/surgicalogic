using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IUserStoreService
    {
        IQueryable<User> GetQueryable();
        Task<ResultModel<UserModel>> GetAsync();
        Task<ResultModel<UserOutputModel>> GetAsync<UserOutputModel>();
        Task<ResultModel<UserModel>> InsertAndSaveAsync(UserModel userModel);
        Task<ResultModel<UserOutputModel>> InsertAndSaveAsync<UserOutputModel>(UserModel userModel);
        Task<ResultModel<int>> DeleteByIdAsync(string id);
        Task<ResultModel<UserModel>> UpdatandSaveAsync(UserModel userModel);
    }
}