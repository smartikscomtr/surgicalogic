using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores
{
    public interface IUserStoreService
    {
        IQueryable<User> GetQueryable();
        Task<ResultModel<UserModel>> FindByIdAsync(int id);
        Task<ResultModel<UserModel>> GetAsync();
        Task<ResultModel<UserOutputModel>> GetAsync<UserOutputModel>();
        Task<List<UserExportModel>> GetExportAsync<UserExportModel>();
        Task<ResultModel<UserModel>> InsertAndSaveAsync(UserModel userModel);
        Task<ResultModel<UserOutputModel>> InsertAndSaveAsync<UserOutputModel>(UserModel userModel);
        Task<ResultModel<int>> DeleteByIdAsync(int id);
        Task<ResultModel<UserModel>> UpdateAndSaveAsync(UserModel userModel);
        Task<bool> IsDuplicateEmail(string email, int id);
    }
}