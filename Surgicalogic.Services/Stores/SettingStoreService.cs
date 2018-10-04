using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Surgicalogic.Services.Stores
{
    public class SettingStoreService : StoreService<Setting, SettingModel>, ISettingStoreService
    {
        private DataContext _context;
        public SettingStoreService(DataContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<SettingModel> GetByKeyAsync(string key)
        {
            return await GetQueryable().Where(x => x.Key == key).ProjectTo<SettingModel>().SingleOrDefaultAsync();
        }

        public async Task<List<SettingModel>> GetAllAsync()
        {
            return await GetQueryable().ProjectTo<SettingModel>().ToListAsync();
        }
    }
}