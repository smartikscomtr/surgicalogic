using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;
using Surgicalogic.Model.InputModel;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores.Base
{
    public interface IStoreService<TEntity, TModel> where TEntity : class where TModel : class
    {
        IQueryable<TEntity> GetQueryable();
        Task<ResultModel<TModel>> FindByIdAsync(int id);
        Task<ResultModel<TModel>> GetAsync(GridInputModel input);
        Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>();
        Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>(GridInputModel input);
        Task<TEntity> InsertAsync(TModel model);
        Task<ResultModel<TModel>> InsertAndSaveAsync(TModel model);
        Task<ResultModel<TOutputModel>> InsertAndSaveAsync<TOutputModel>(TModel model);
        Task<TEntity> DeleteByIdAsync(int id);
        Task<ResultModel<int>> DeleteAndSaveByIdAsync(int id);
        Task<TEntity> UpdateAsync(TModel model);
        Task<ResultModel<TModel>> UpdateAndSaveAsync(TModel model);
        Task<ResultModel<TOutputModel>> UpdateAndSaveAsync<TOutputModel>(TModel model);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}