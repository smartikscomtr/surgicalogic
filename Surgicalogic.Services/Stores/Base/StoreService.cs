using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;
using Surgicalogic.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores.Base
{
    public abstract class StoreService<TEntity, TModel>  : IStoreService<TEntity, TModel> 
        where TEntity : Entity
        where TModel : EntityModel
        
    {

        private DataContext _context;

        protected StoreService(DataContext context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<ResultModel<TModel>> GetAsync()
        {
            var query = GetQueryable();

            var projectQuery = query.ProjectTo<TModel>();

            int totalCount = await projectQuery.CountAsync();

            var result = await projectQuery.ToListAsync();

            return new ResultModel<TModel>
            {
                Result = result,
                TotalCount = totalCount,
                Info = new Info()

            };
        }

        public virtual async Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>()
        {
            var result = await GetAsync();

            return new ResultModel<TOutputModel>
            {
                Result = Mapper.Map<IEnumerable<TOutputModel>>(result.Result),
                TotalCount = result.TotalCount,
                Info = result.Info
            };
        }
        

        public virtual async Task<ResultModel<TModel>> InsertAndSaveAsync(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);

            entity.CreatedBy = 2;

            entity.CreatedDate = DateTime.Now;

            _context.Set<TEntity>().Add(entity);

            await _context.SaveChangesAsync();
            
            return new ResultModel<TModel>
            {
                Result = entity,
                Info = new Info()
            };
        }

        public virtual async Task<ResultModel<int>> DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FirstAsync(e => e.Id == id);

            _context.Set<TEntity>().Remove(entity);

            await _context.SaveChangesAsync();

            return new ResultModel<int>
            {
                Result = null,
                Info = new Info()
            };
        }

        public virtual async Task<ResultModel<TModel>> UpdatandSaveAsync(TModel model)
        {
            var entity = await _context.Set<TEntity>().FirstAsync(e => e.Id == model.Id);

            Mapper.Map(model, entity);

            entity.ModifiedBy = 2;

            entity.ModifiedDate = DateTime.Now;

            return new ResultModel<TModel>
            {
                Result = model,
                Info = new Info()
            };
        }



    }
}
