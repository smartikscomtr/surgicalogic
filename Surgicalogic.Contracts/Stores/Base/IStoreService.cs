using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;

namespace Surgicalogic.Contracts.Stores.Base
{
    public interface IStoreService<TModel, TSorting, TFilter>
        where TModel : EntityModel
        where TSorting : struct
        where TFilter : struct
    {
        Task<ResultModel<TModel>> GetAsync(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination);
        Task<TModel> FirstOrDefaultAsync(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination);
        Task<TModel> FindByIdAsync(int id);
        Task<int> InsertAsync(TModel model);
        Task<int> DeleteByIdAsync(int id);
        Task UpdateAsync(TModel model);
        Task UpdateAsync(IEnumerable<(string Column, object Value)> set, IEnumerable<(string Column, object Value)> where);
    }
}
