using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel.Base;

namespace Surgicalogic.Contracts.Stores.Base
{
    public interface IStoreService<TModel, TSorting, TFilter>
        where TModel : class
        where TSorting : struct
        where TFilter : struct
    {
        Task<ResultModel<TModel>> GetAsync(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination);
        Task<ResultModel<TOutputModel>> GetAsync<TOutputModel>(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination);
        Task<TModel> FirstOrDefaultAsync(FilterSortPaginationModel<TSorting, TFilter> filterSortPagination);
        Task<TModel> FindByIdAsync(int id);
        Task<ResultModel<TModel>> InsertAsync(TModel model);
        Task<ResultModel<int>> DeleteByIdAsync(int id);
        Task<ResultModel<TModel>> UpdateAsync(TModel model);
        Task UpdateAsync(IEnumerable<(string Column, object Value)> set, IEnumerable<(string Column, object Value)> where);
    }
}
