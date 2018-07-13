using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class UserStoreService : IUserStoreService
    {
        private DataContext _context;
        public UserStoreService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This methode returns an IQuerable entity list being active. 
        /// </summary>
        /// <returns>IQueryable</returns>
        public virtual IQueryable<User> GetQueryable()
        {
            return _context.Set<User>().AsNoTracking();
        }

        /// <summary>
        ///This methode returns list of specifed type model list.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<UserModel>> GetAsync()
        {
            var query = GetQueryable();
            var projectQuery = query.ProjectTo<UserModel>();
            int totalCount = await projectQuery.CountAsync();
            var result = await projectQuery.ToListAsync();

            return new ResultModel<UserModel>
            {
                Result = result,
                TotalCount = totalCount,
                Info = new Info()
            };
        }

        /// <summary>
        ///This methode returns list of specifed type model list.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <returns>ResultModel</returns>
        public async Task<ResultModel<UserOutputModel>> GetAsync<UserOutputModel>()
        {
            var result = await GetAsync();

            return new ResultModel<UserOutputModel>
            {
                Result = Mapper.Map<IEnumerable<UserOutputModel>>(result.Result),
                TotalCount = result.TotalCount,
                Info = result.Info
            };
        }

        /// <summary>
        ///This methode returns list of specifed type model list.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <returns>ResultModel</returns>
        public async Task<ResultModel<UserModel>> InsertAndSaveAsync(UserModel userModel)
        {
            var entity = Mapper.Map<User>(userModel);

            entity.EmailConfirmed = false;
            entity.PhoneNumberConfirmed = false;
            entity.TwoFactorEnabled = false;
            entity.LockoutEnabled = false;
            entity.AccessFailedCount = 0;

            await _context.Set<User>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return new ResultModel<UserModel>
            {
                Result = GetQueryable().ProjectTo<UserModel>().First(e => e.Id == entity.Id),
                Info = new Info()
            };
        }

        /// <summary>
        /// This methode adds a new entity record with entity type and retruns specifed type of record.
        /// </summary>
        /// <typeparam name="TOutputModel"></typeparam>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public virtual async Task<ResultModel<UserOutputModel>> InsertAndSaveAsync<UserOutputModel>(UserModel model)
        {
            var result = await InsertAndSaveAsync(model);

            return new ResultModel<UserOutputModel>
            {
                Result = Mapper.Map<UserOutputModel>(result.Result),
                Info = result.Info
            };
        }

        /// <summary>
        /// This methode deletes entity by marking false IsActive value of entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResultModel</returns>
        public async Task<ResultModel<int>> DeleteByIdAsync(string id)
        {
            var entity = await _context.Set<User>().FirstAsync(e => e.Id == id);

            //entity.IsActive = false;
            //entity.ModifiedBy = 0;
            //entity.ModifiedDate = DateTime.Now;

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
        public async Task<ResultModel<UserModel>> UpdatandSaveAsync(UserModel userModel)
        {
            var entity = await _context.Set<User>().FirstAsync(e => e.Id == userModel.Id);

            Mapper.Map(userModel, entity);

            //entity.ModifiedBy = 2;
            //entity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ResultModel<UserModel>
            {
                Result = userModel,
                Info = new Info()
            };
        }
    }
}