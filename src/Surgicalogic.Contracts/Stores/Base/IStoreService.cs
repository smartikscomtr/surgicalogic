﻿using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Stores.Base
{
    public interface IStoreService<TEntity, TModel> where TEntity : class where TModel : class
    {
        IQueryable<TEntity> GetQueryable();
        Task<ResultModel<TModel>> FindByIdAsync(int id);
        Task<ResultModel<TModel>> GetAsync(GridInputModel input, Expression<Func<TEntity, bool>> expression);
        Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>();
        Task<List<TExportModel>> GetExportAsync<TExportModel>();
        Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>(GridInputModel input, Expression<Func<TEntity, bool>> expression = null);
        Task<TEntity> InsertAsync(TModel model);
        Task<ResultModel<TModel>> InsertAndSaveAsync(TModel model);
        Task<ResultModel<TOutputModel>> InsertAndSaveAsync<TOutputModel>(TModel model);
        Task<ResultModel<TEntity>> DeleteByIdAsync(int id);
        Task<ResultModel<int>> DeleteAndSaveByIdAsync(int id);
        Task<TEntity> UpdateAsync(TModel model);
        Task<ResultModel<TModel>> UpdateAndSaveAsync(TModel model);
        Task<ResultModel<TOutputModel>> UpdateAndSaveAsync<TOutputModel>(TModel model);
        Task<int> SaveChangesAsync();
    }
}