using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Stores.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class WorkTypeStoreService : StoreService<WorkType, WorkTypeModel>, IWorkTypeStoreService
    {
        private DataContext _context;
        public WorkTypeStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ResultModel<int>> DeleteAndSaveByIdAsync(int id)
        {
            //if (_context.Personnels.Any(x => x.WorkTypeId == id && x.IsActive))
            //{
            //    return new ResultModel<int>
            //    {
            //        Info = new Info
            //        {
            //            InfoType = Model.Enum.InfoType.Error,
            //            Message = Model.Enum.MessageType.ModelHasRelationalData
            //        }
            //    };
            //}

            return await base.DeleteAndSaveByIdAsync(id);
        }
    }
}