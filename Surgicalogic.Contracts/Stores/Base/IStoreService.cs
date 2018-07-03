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

        //T Add(T t);
        //Task<T> AddAsyn(T t);
        //int Count();
        //Task<int> CountAsync();
        //void Delete(T entity);
        //Task<int> DeleteAsyn(T entity);
        //void Dispose();
        //T Find(Expression<Func<T, bool>> match);
        //ICollection<T> FindAll(Expression<Func<T, bool>> match);
        //Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        //Task<T> FindAsync(Expression<Func<T, bool>> match);
        //IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        //Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        //T Get(int id);
        //IQueryable<T> GetAll();
        //Task<ICollection<T>> GetAllAsyn();
        //IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        //Task<T> GetAsync(int id);
        //void Save();
        //Task<int> SaveAsync();
        //T Update(T t, object key);
        //Task<T> UpdateAsyn(T t, object key);
    }
}
