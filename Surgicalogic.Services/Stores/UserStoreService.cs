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

        public virtual IQueryable<User> GetQueryable()
        {
            return _context.Set<User>().AsNoTracking();
        }

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

        public async Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>()
        {
            var result = await GetAsync();

            return new ResultModel<TOutputModel>
            {
                Result = Mapper.Map<IEnumerable<TOutputModel>>(result.Result),
                TotalCount = result.TotalCount,
                Info = result.Info
            };
        }

    }
}
