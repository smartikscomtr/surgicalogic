using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;
using Surgicalogic.Model.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores.Base
{
    public abstract class StoreService<TEntity, TModel> where TEntity : Entity where TModel : EntityModel
    {
        private DataContext _context;

        protected StoreService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This methode returns an IQuerable entity list being active. 
        /// </summary>
        /// <returns>IQueryable</returns>
        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>().AsNoTracking().Where(i => i.IsActive);
        }

        /// <summary>
        /// This methode returns list of entity model.
        /// </summary>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TModel>> GetAsync(GridInputModel input)
        {
            var query = GetQueryable();
            var projectQuery = query.ProjectTo<TModel>();

            //switch (order)
            //{
            //    case order.Asc:
            //        projectQuery = projectQuery.OrderBy();

            //        break;
            //    default:
            //        break;
            //}

            if (!string.IsNullOrEmpty(input.Search))
            {
                List<string> searchableProperties = Common.QueryService<TModel>.GetSearchableProperties();

                if (searchableProperties.Count > 0)
                {
                    var lambda = Common.QueryService<TModel>.GetSearchQuery(searchableProperties, input.Search);
                    projectQuery = projectQuery.Where(lambda);
                }
            }

            int totalCount = await projectQuery.CountAsync();

            if (input.PageSize > 0)
            {
                projectQuery = projectQuery.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);
            }

           
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
            return await GetAsync<TOutputModel>(new GridInputModel { PageSize = 0 });
        }

        /// <summary>
        ///This methode returns list of specifed type model list.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>(GridInputModel input)
        {
            var result = await GetAsync(input);

            return new ResultModel<TOutputModel>
            {
                Result = Mapper.Map<IEnumerable<TOutputModel>>(result.Result),
                TotalCount = result.TotalCount,
                Info = result.Info
            };
        }

        /// <summary>
        /// This methode adds a new entity record with entity type.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TModel>> InsertAndSaveAsync(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);

            entity.CreatedBy = 2;
            entity.CreatedDate = DateTime.Now;

            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return new ResultModel<TModel>
            {
                Result = GetQueryable().ProjectTo<TModel>().First(e => e.Id == entity.Id),
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode adds a new entity record with entity type and retruns specifed type of record.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TOutputModel>> InsertAndSaveAsync<TOutputModel>(TModel model)
        {
            var result = await InsertAndSaveAsync(model);

            return new ResultModel<TOutputModel>
            {
                Result = Mapper.Map<TOutputModel>(result.Result),
                Info = result.Info
            };
        }

        /// <summary>
        /// This methode deletes entity by marking false IsActive value of entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<int>> DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FirstAsync(e => e.Id == id);

            entity.IsActive = false;
            entity.ModifiedBy = 0;
            entity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ResultModel<int>
            {
                Result = null,
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode modifies the entities properties and saves it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TModel>> UpdateAndSaveAsync(TModel model)
        {
            var entity = await _context.Set<TEntity>().FirstAsync(e => e.Id == model.Id);

            Mapper.Map(model, entity);

            entity.ModifiedBy = 2;
            entity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ResultModel<TModel>
            {
                Result = model,
                Info = new Info()
            };
        }
    }
}