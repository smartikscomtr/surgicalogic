using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Services.Stores.Base;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores
{
    public class OperatingRoomStoreService : StoreService<OperatingRoom, OperatingRoomModel>, IOperatingRoomStoreService
    {
        private DataContext _context;
        public OperatingRoomStoreService(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel<OperatingRoomModel>> UpdateAndSaveOperatingRoomAsync(OperatingRoomInputModel item)
        {
            var operatingRoomItem = new OperatingRoomModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Location = item.Location,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length
            };

            return new ResultModel<OperatingRoomModel>();
        }


    }
}