using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;

namespace Surgicalogic.Contracts.Stores.Base
{
    public interface IStoreService<TEntity, TModel>
        where TEntity : Entity
        where  TModel : EntityModel
    {
        IQueryable<TEntity> GetQueryable();

        Task<ResultModel<TModel>> GetAsync();
        Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>();        
        Task<ResultModel<TModel>> InsertAndSaveAsync(TModel model);
        Task<ResultModel<int>> DeleteByIdAsync(int id);
        Task<ResultModel<TModel>> UpdatandSaveAsync(TModel model);
    }
}
