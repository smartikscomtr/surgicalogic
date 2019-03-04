using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Surgicalogic.Services.Stores
{
    public class EquipmentStoreService : StoreService<Equipment, EquipmentModel>, IEquipmentStoreService
    {
        private DataContext _context;
        public EquipmentStoreService(DataContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<ResultModel<EquipmentOutputModel>> GetNonPortableEquipments()
        {
            var query = await GetQueryable().Where(x => !x.IsPortable).ProjectTo<EquipmentOutputModel>().ToListAsync();           

            return new ResultModel<EquipmentOutputModel>
            {
                Result = query,
                TotalCount = query.Count(),
                Info = new Info()
            };
        }

        public async Task<bool> IsDuplicateCode(string code, int id)
        {
            return await GetQueryable().AnyAsync(x => x.Code == code && x.Id != id);
        }
    }
}