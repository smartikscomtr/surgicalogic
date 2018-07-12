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
    }
}