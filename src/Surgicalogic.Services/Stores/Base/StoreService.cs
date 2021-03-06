﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Services.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// This methode returns the model which has given id.
        /// </summary>       
        /// <returns>TModel</returns>
        public virtual async Task<ResultModel<TModel>> FindByIdAsync(int id)
        {
            var query = GetQueryable();
            var model = await query.ProjectTo<TModel>().FirstOrDefaultAsync(x => x.Id == id);

            return new ResultModel<TModel>
            {
                Result = model,
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode returns list of entity model.
        /// </summary>
        /// <param name="GridInputModel">GridInputModel</param>  
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TModel>> GetAsync(GridInputModel input, Expression<Func<TEntity, bool>> expression)
        {
            var query = GetQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            var projectQuery = query.ProjectTo<TModel>();

            if (!string.IsNullOrEmpty(input.Search))
            {
                List<string> searchableProperties = QueryFilterService<TModel>.GetSearchableProperties();

                if (searchableProperties.Count > 0)
                {
                    var lambda = QueryFilterService<TModel>.GetSearchQuery(searchableProperties, input.Search);
                    projectQuery = projectQuery.Where(lambda);
                }
            }

            if (!string.IsNullOrEmpty(input.SortBy))
            {
                if (input.Descending == true)
                {
                    projectQuery = projectQuery.OrderByPropertyDescending(input.SortBy);
                }
                else
                {
                    projectQuery = projectQuery.OrderByProperty(input.SortBy);
                }
            }

            int totalCount = await projectQuery.CountAsync();

            if (input.PageSize > 0)
            {
                projectQuery = projectQuery.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);
            }

            var result = new List<TModel>();
            try
            {
                result = await projectQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                //TODO: EF Core sürümü oluşturduğumuz sorguyu bazı istisnai durumlarda async olarak çalıştırmıyor. Geçici olarak bu şekilde yaptım, yeni sürümlerde ya da sorguyu değiştirerek inceleyeceğim.  
                if (ex.Message.Contains("IAsyncEnumerable"))
                {
                    result = projectQuery.ToList();
                }
            }

            return new ResultModel<TModel>
            {
                Result = result,
                TotalCount = totalCount,
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode returns list of specifed type model list.
        /// </summary>       
        /// <returns>TOutputModel</returns>
        public virtual async Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>()
        {
            return await GetAsync<TOutputModel>(new GridInputModel { PageSize = 0 });
        }

        /// <summary>
        /// This methode returns list of specifed type model list.
        /// </summary>       
        /// <returns>TOutputModel</returns>
        public virtual async Task<List<TExportModel>> GetExportAsync<TExportModel>()
        {
            var query = GetQueryable();
            var projectQuery = query.ProjectTo<TExportModel>();

            return await projectQuery.ToListAsync();
        }

        /// <summary>
        ///This methode returns list of specifed type model list.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <returns>TOutputModel</returns>
        public virtual async Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>(GridInputModel input, Expression<Func<TEntity, bool>> expression = null)
        {
            var result = await GetAsync(input, expression);

            return new ResultModel<TOutputModel>
            {
                Result = Mapper.Map<IEnumerable<TOutputModel>>(result.Result),
                TotalCount = result.TotalCount,
                Info = result.Info
            };
        }

        /// <summary>
        /// This methode adds a new entity record with entity type. This method doesn't save changes.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<TEntity> InsertAsync(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);

            entity.CreatedBy = 2;
            entity.CreatedDate = DateTime.Now;

            await _context.Set<TEntity>().AddAsync(entity);

            return entity;
        }

        /// <summary>
        /// This methode adds and saves a new entity record with entity type.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TModel>> InsertAndSaveAsync(TModel model)
        {
            var entity = await InsertAsync(model);

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
        /// This methode deletes entity by marking false IsActive value of entity. This method doesn't save changes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TEntity>> DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FirstAsync(e => e.Id == id);

            var hasDependentData = await DependentDataService.CheckDependentAttributes(_context, entity.GetType(), id);

            if (hasDependentData)
            {
                return new ResultModel<TEntity>
                {
                    Result = entity,
                    Info = new Info
                    {
                        Succeeded = false,
                        Message = Model.Enum.MessageType.ModelHasRelationalData,
                        InfoType = Model.Enum.InfoType.Error
                    }
                };
            }

            entity.IsActive = false;
            entity.ModifiedBy = 0;
            entity.ModifiedDate = DateTime.Now;

            return new ResultModel<TEntity>
            {
                Result = entity,
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode deletes entity by marking false IsActive value of entity and saves changes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<int>> DeleteAndSaveByIdAsync(int id)
        {
            var result = new ResultModel<int>
            {
                Result = null,
                Info = new Info()
            };

            var entity = await DeleteByIdAsync(id);

            if (!entity.Info.Succeeded)
            {
                result.Info = entity.Info;
                return result;
            }

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// This methode modifies the entities properties and doesn't save it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<TEntity> UpdateAsync(TModel model)
        {
            var entity = await _context.Set<TEntity>().FirstAsync(e => e.Id == model.Id);

            Mapper.Map(model, entity);

            entity.ModifiedBy = 2;
            entity.ModifiedDate = DateTime.Now;

            return entity;
        }

        /// <summary>
        /// This methode modifies the entities properties and saves it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<TModel>> UpdateAndSaveAsync(TModel model)
        {
            var entity = await UpdateAsync(model);

            await _context.SaveChangesAsync();

            return new ResultModel<TModel>
            {
                Result = GetQueryable().ProjectTo<TModel>().First(e => e.Id == entity.Id),
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode modifies the entities properties and saves it.
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <returns>TOutputModel</returns>
        public virtual async Task<ResultModel<TOutputModel>> UpdateAndSaveAsync<TOutputModel>(TModel model)
        {
            var result = await UpdateAndSaveAsync(model);
            return new ResultModel<TOutputModel>
            {
                Result = Mapper.Map<TOutputModel>(result.Result),
                Info = result.Info
            };
        }

        /// <summary>
        /// This method saves changes asynchronous.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}