using System.Collections.Generic;

namespace Surgicalogic.Model.CommonModel
{
    public class FilterSortPaginationModel<TSorting, TFilter, TFilterValue> where TSorting : struct where TFilter : struct
    {
        public string Search { get; set; }
        public TSorting? Sorting { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public IEnumerable<FilterModel<TFilter, TFilterValue>> Filters { get; set; }
    }

    public class FilterSortPaginationModel<TSorting, TFilter> : FilterSortPaginationModel<TSorting, TFilter, object> where TSorting : struct where TFilter : struct
    {
    }
}